Feature: Folder synchronization

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