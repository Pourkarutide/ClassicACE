#!/bin/bash
base_branch="merge-dekaru-fork"
upstream="dekaru/master"
batch_size=10

# Ensure base branch exists
git checkout $base_branch || { echo "Failed to checkout $base_branch. Create it first."; exit 1; }

# Fetch latest data from upstream
git fetch dekaru || { echo "Failed to fetch dekaru"; exit 1; }

# Find the merge base between base_branch and upstream
base=$(git merge-base $base_branch $upstream)
if [ -z "$base" ]; then
  echo "Couldn’t find merge base between $base_branch and $upstream"
  exit 1
fi

# Get all commits from upstream since the base, oldest first
commits=($(git log $base..$upstream --reverse --oneline | awk '{print $1}'))
total=${#commits[@]}

if [ $total -eq 0 ]; then
  echo "No commits to merge from $upstream"
  exit 0
fi

echo "Total commits to merge: $total"

# Find the last batch branch that exists and has a successful merge
last_batch_num=0
for ((i=0; i<total; i+=batch_size)); do
  batch_num=$((i / batch_size + 1))
  batch_branch="merge-dekaru-batch-$batch_num"
  if git show-ref --quiet refs/heads/$batch_branch; then
    # Check if this branch has a merge commit from upstream
    last_commit_in_batch=${commits[$((i + batch_size - 1 < total ? i + batch_size - 1 : total - 1))]}
    if git merge-base --is-ancestor "$last_commit_in_batch" $batch_branch; then
      last_batch_num=$batch_num
    fi
  else
    break
  fi
done

start_index=$((last_batch_num * batch_size))
echo "Resuming from batch $((last_batch_num + 1)) (index $start_index)"

# Process batches from the starting point
for ((i=start_index; i<total; i+=batch_size)); do
  end=$((i + batch_size - 1))
  if [ $end -ge $total ]; then end=$((total - 1)); fi
  sha=${commits[$end]}
  batch_num=$((i / batch_size + 1))
  batch_branch="merge-dekaru-batch-$batch_num"
  start_num=$((i + 1))
  end_num=$((end + 1))

  echo "Creating and merging batch $batch_num: commits $start_num-$end_num ($sha)"
  
  # Create and checkout new batch branch from base_branch
  git checkout -b $batch_branch $base_branch 2>/dev/null || git checkout $batch_branch
  
  # Merge the batch
  git merge --no-ff $sha -m "Merge Dekaru commits $start_num-$end_num into $batch_branch"
  if [ $? -ne 0 ]; then
    echo "Merge conflicts detected in $batch_branch. Resolve conflicts, commit, then rerun the script."
    echo "After resolving, switch back to $base_branch before restarting."
    exit 1
  fi

  echo "Batch $batch_num merged successfully into $batch_branch!"
  echo "To create a PR, push with: git push origin $batch_branch"
  
  # Switch back to base_branch for the next iteration
  git checkout $base_branch
done

echo "All batches merged into separate branches! Create PRs for each merge-dekaru-batch-X branch."
