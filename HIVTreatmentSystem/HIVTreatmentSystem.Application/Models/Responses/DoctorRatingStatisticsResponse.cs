using System.ComponentModel.DataAnnotations;

namespace HIVTreatmentSystem.Application.Models.Responses
{
    /// <summary>
    /// Response model for doctor rating statistics
    /// </summary>
    public class DoctorRatingStatisticsResponse
    {
        /// <summary>
        /// Doctor ID
        /// </summary>
        public int DoctorId { get; set; }

        /// <summary>
        /// Average rating of the doctor
        /// </summary>
        public double AverageRating { get; set; }

        /// <summary>
        /// Total number of feedbacks
        /// </summary>
        public int TotalFeedbacks { get; set; }

        /// <summary>
        /// Number of 1-star ratings
        /// </summary>
        public int OneStarCount { get; set; }

        /// <summary>
        /// Number of 2-star ratings
        /// </summary>
        public int TwoStarCount { get; set; }

        /// <summary>
        /// Number of 3-star ratings
        /// </summary>
        public int ThreeStarCount { get; set; }

        /// <summary>
        /// Number of 4-star ratings
        /// </summary>
        public int FourStarCount { get; set; }

        /// <summary>
        /// Number of 5-star ratings
        /// </summary>
        public int FiveStarCount { get; set; }

        /// <summary>
        /// Percentage distribution of ratings
        /// </summary>
        public RatingDistributionResponse RatingDistribution { get; set; } = new();
    }

    /// <summary>
    /// Rating distribution percentages
    /// </summary>
    public class RatingDistributionResponse
    {
        /// <summary>
        /// Percentage of 1-star ratings
        /// </summary>
        public double OneStarPercentage { get; set; }

        /// <summary>
        /// Percentage of 2-star ratings
        /// </summary>
        public double TwoStarPercentage { get; set; }

        /// <summary>
        /// Percentage of 3-star ratings
        /// </summary>
        public double ThreeStarPercentage { get; set; }

        /// <summary>
        /// Percentage of 4-star ratings
        /// </summary>
        public double FourStarPercentage { get; set; }

        /// <summary>
        /// Percentage of 5-star ratings
        /// </summary>
        public double FiveStarPercentage { get; set; }
    }
} 