Feature: Folder synchronization

  Scenario: Copy files from source folder to replica folder
    Given a source folder
    And the source folder contains the following files:
      | file            |
      | E2E-TC-1.txt    |
      | E2E-TC-1.csv    |
      | E2E-TC-1.png    |
    And an empty replica folder
    When I run the folder synchronizer
    Then the source folder contains the following files:
      | file            |
      | E2E-TC-1.txt    |
      | E2E-TC-1.csv    |
      | E2E-TC-1.png    |
