﻿using System;

namespace FreeYourFridgeAPI.Tests.IntegrationTestsHelper.TestModels
{
    public class ExpectedDailyMealDetailedDtoTestModel
    {
        public Guid LocalId { get; set; }

        public string Title { get; set; }
        public string Image { get; set; }
        public DateTime TimeOfLastMeal { get; set; }

        public int Id { get; set; }
        public int Grams { get; set; }
        public string? UserRemarks { get; set; }
        public int CaloriesPerPortion { get; set; }
        public int Carbs { get; set; }
        public int Fat { get; set; }
        public int Protein { get; set; }
        public int CreatedBy { get; set; }
    }
}
