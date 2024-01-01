DELETE FROM `quest` WHERE `name` = 'ExplorationAssignmentsGiven';

INSERT INTO `quest` (`name`, `min_Delta`, `max_Solves`, `message`, `last_Modified`)
VALUES ('ExplorationAssignmentsGiven', 43200, -1, 'Player has been given exploration assignments', '2023-06-25 06:23:17');
