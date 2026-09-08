Feature: Folder synchronization

Acceptance Criterias:

AC-1: Synchronization must be one-way: after the synchronization content of the replica
folder should be modified to exactly match content of the source folder;

AC-2: Synchronization should be performed periodically;
AC-3: File creation/copying/removal operations should be logged to a file and to the
console output;
AC-4: Folder paths, synchronization interval and log file path should be provided using
the command line arguments;

@AC-1
@AC-3
@AC-4
  Scenario: Initial synchronization copies source contents to replica
    Given a source folder
    And the source folder contains the following files:
      | file            |
      | E2E-TC-1.txt    |
      | E2E-TC-1.csv    |
      | E2E-TC-1.png    |
    And the source folder contains the following folders:
      | folder              |
      | empty-folder        |
      | nested-1/nested-2   |
    And the folder "nested-1/nested-2" in the source contains the following files:
      | file         |
      | E2E-TC-1.txt |
      | E2E-TC-1.csv |
    And an empty replica folder
    When I run the folder synchronizer
    Then the replica folder contains the following folders:
      | folder            |
      | empty-folder      |
      | nested-1/nested-2 |
    And the replica folder contains the following files:
      | file            |
      | E2E-TC-1.txt    |
      | E2E-TC-1.csv    |
      | E2E-TC-1.png    |
    And the folder "nested-1/nested-2" in the replica contains the following files:
		| file         |
		| E2E-TC-1.txt |
		| E2E-TC-1.csv |
	And the log file is generated and is not empty

@AC-1
@AC-2
@AC-3
@AC-4
Scenario: Synchronization should be performed periodically
     Given a source folder
     And the source folder contains the following files:
        | file         |
        | E2E-TC-1.txt |
     And an empty replica folder
     When I run the folder synchronizer with sync interval 3 seconds
     Then the replica folder contains the following files:
        | file          |
        | E2E-TC-1.txt  |
     When I rename the following files in the source folder:
        | original      | new                   |
        | E2E-TC-1.txt  | E2E-TC-1-UPDATED.txt  |
     And I wait 3 seconds
     Then the replica folder contains the following files:
        | file                  |
        | E2E-TC-1-UPDATED.txt  |
     And the replica folder does not contain the following files:
        | file         |
        | E2E-TC-1.txt |
    And the log file is generated and is not empty

@AC-1
@AC-3
@AC-4
Scenario: Obsolete files and folders are removed from replica
    Given a source folder
    And the source folder contains the following files:
        | file         |
        | E2E-TC-1.txt |
    And an empty replica folder
    And the replica folder contains the following files:
        | file              |
        | obsolete-file.txt |
    And the replica folder contains the following folders:
        | folder                    |
        | obsolete-folder           |
        | obsolete-folder/nested    |
    When I run the folder synchronizer
    Then the replica folder contains the following files:
        | file         |
        | E2E-TC-1.txt |
    And the replica folder does not contain the following files:
        | file              |
        | obsolete-file.txt |
    And the replica folder does not contain the following folders:
        | folder          |
        | obsolete-folder |
    And the log file is generated and is not empty    

@AC-1
@AC-3
@AC-4    
Scenario: Changed file content is synchronized to replica
  Given a source folder
  And the source folder contains the following files:
    | file         |
    | E2E-TC-1.txt |
  And an empty replica folder
  When I run the folder synchronizer with sync interval 1 seconds
  Then the replica folder contains the following files:
      | file            |
      | E2E-TC-1.txt    |
  When I change the content of the following file in the source folder:
    | file         | New file content                           |
    | E2E-TC-1.txt | This is updated test file content 123!@#   |
  Then the content of the file "E2E-TC-1.txt" in the replica should be:
	"""
	This is updated test file content 123!@#
	"""
