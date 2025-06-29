using AutoMapper;
using System;
using System.Linq;
using System.Collections.Generic;
using HIVTreatmentSystem.Application.Interfaces;
using HIVTreatmentSystem.Application.Models.Requests;
using HIVTreatmentSystem.Application.Models.Responses;
using HIVTreatmentSystem.Application.Repositories;
using HIVTreatmentSystem.Domain.Entities;

namespace HIVTreatmentSystem.Application.Services
{
    /// <summary>
    /// Implementation of feedback business logic
    /// </summary>
    public class FeedbackService : IFeedbackService
    {
        private readonly IFeedbackRepository _repository;
        private readonly IMapper _mapper;

        public FeedbackService(IFeedbackRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<ListFeedbacksResponse> GetPagedAsync(ListFeedbacksRequest request)
        {
            var (items, total) = await _repository.GetPagedAsync(
                request.PatientId,
                request.AppointmentId,
                request.Rating,
                request.PageNumber,
                request.PageSize);

            var responses = _mapper.Map<IEnumerable<FeedbackResponse>>(items).ToList();

            return new ListFeedbacksResponse { Feedbacks = responses, TotalCount = total };
        }

        public async Task<FeedbackResponse> GetByIdAsync(int id)
        {
            var entity = await _repository.GetByIdAsync(id);
            if (entity == null)
                throw new KeyNotFoundException("không tìm thấy feedback");
            return _mapper.Map<FeedbackResponse>(entity);
        }

        public async Task<FeedbackResponse> CreateAsync(FeedbackRequest request)
        {
            var entity = _mapper.Map<Feedback>(request);
            entity.CreatedAt = DateTime.UtcNow;
            var saved = await _repository.AddAsync(entity);
            return _mapper.Map<FeedbackResponse>(saved);
        }

        public async Task<FeedbackResponse> UpdateAsync(int id, FeedbackRequest request)
        {
            var entity = await _repository.GetByIdAsync(id);
            if (entity == null)
                throw new KeyNotFoundException("không tìm thấy feedback");
            _mapper.Map(request, entity);
            var updated = await _repository.UpdateAsync(entity);
            return _mapper.Map<FeedbackResponse>(updated);
        }

        public async Task DeleteAsync(int id)
        {
            var success = await _repository.DeleteAsync(id);
            if (!success)
                throw new KeyNotFoundException("không tìm thấy feedback");
        }

        public async Task<DoctorRatingStatisticsResponse> GetDoctorRatingStatisticsAsync(int doctorId)
        {
            var (averageRating, totalFeedbacks, oneStarCount, twoStarCount, threeStarCount, fourStarCount, fiveStarCount) 
                = await _repository.GetDoctorRatingStatisticsAsync(doctorId);

            var response = new DoctorRatingStatisticsResponse
            {
                DoctorId = doctorId,
                AverageRating = Math.Round(averageRating, 2),
                TotalFeedbacks = totalFeedbacks,
                OneStarCount = oneStarCount,
                TwoStarCount = twoStarCount,
                ThreeStarCount = threeStarCount,
                FourStarCount = fourStarCount,
                FiveStarCount = fiveStarCount,
                RatingDistribution = new RatingDistributionResponse()
            };

            // Calculate percentages if there are feedbacks
            if (totalFeedbacks > 0)
            {
                response.RatingDistribution.OneStarPercentage = Math.Round((double)oneStarCount / totalFeedbacks * 100, 2);
                response.RatingDistribution.TwoStarPercentage = Math.Round((double)twoStarCount / totalFeedbacks * 100, 2);
                response.RatingDistribution.ThreeStarPercentage = Math.Round((double)threeStarCount / totalFeedbacks * 100, 2);
                response.RatingDistribution.FourStarPercentage = Math.Round((double)fourStarCount / totalFeedbacks * 100, 2);
                response.RatingDistribution.FiveStarPercentage = Math.Round((double)fiveStarCount / totalFeedbacks * 100, 2);
            }

            return response;
        }
    }
} 