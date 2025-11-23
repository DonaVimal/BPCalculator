@bloodpressure
Feature: Blood Pressure Category Evaluation
  To ensure accurate BP classification
  As a health application
  I want to determine the correct BP category based on systolic and diastolic values

  @category
  Scenario: Classify valid blood pressure values
    Given my systolic value is <Systolic>
    And my diastolic value is <Diastolic>
    When I evaluate the BP category
    Then the result should be "<Expected>"

    Examples:
      | Systolic | Diastolic | Expected |
      | 85       | 55        | Low      |
      | 95       | 65        | Ideal    |
      | 118      | 78        | Ideal    |
      | 125      | 79        | PreHigh  |
      | 139      | 85        | PreHigh  |
      | 140      | 88        | High     |
      | 150      | 95        | High     |

  @invalid-reading
  Scenario: Systolic must be higher than diastolic
    Given my systolic value is <Systolic>
    And my diastolic value is <Diastolic>
    When I evaluate the BP category
    Then an invalid reading error should be thrown

    Examples:
      | Systolic | Diastolic |
      | 80       | 80        |
      | 90       | 100       |
      | 120      | 130       |

  @out-of-range
  Scenario: Validate systolic and diastolic input range
    Given my systolic value is <Systolic>
    And my diastolic value is <Diastolic>
    When I validate the BP input range
    Then the validation should fail with "<Message>"

    Examples:
      | Systolic | Diastolic | Message                |
      | 60       | 70        | Invalid Systolic Value |
      | 200      | 70        | Invalid Systolic Value |
      | 120      | 30        | Invalid Diastolic Value|
      | 120      | 150       | Invalid Diastolic Value|
