using System;
using System.Threading.Tasks;
using Guandian.Data;
using Guandian.Data.Entity;
using Octokit;

namespace Guandian.Manager
{
    /// <summary>
    /// 审核管理逻辑
    /// </summary>
    public class ReviewManager : BaseManager
    {
        public ReviewManager(ApplicationDbContext context) : base(context)
        {
        }

        /// <summary>
        /// 添加审核
        /// </summary>
        /// <param name="review"></param>
        /// <returns></returns>
        public async Task<Review> AddReviewAsync(Review review)
        {
            await _context.AddAsync(review);
            await _context.SaveChangesAsync();
            return review;
        }

        /// <summary>
        /// PR后添加审核
        /// </summary>
        /// <param name="pullRequest"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        public async Task<Review> AddReviewFromPRAsync(PullRequest pullRequest, Data.Entity.User user)
        {
            var review = new Review
            {
                Body = pullRequest.Body,
                DiffUrl = pullRequest.DiffUrl,
                MergeCommitSha = pullRequest.MergeCommitSha,
                MergeTime = pullRequest.MergedAt ?? DateTimeOffset.Now,
                Number = pullRequest.Number,
                Title = pullRequest.Title,
                Url = pullRequest.Url,
                HtmlUrl = pullRequest.HtmlUrl,
                User = user,
                Merged = pullRequest.Merged,
            };
            review.MergeStatus = pullRequest.State.Value == ItemState.Open
                ? ReviewStatus.Open
                : ReviewStatus.Closed;
            await _context.AddAsync(review);
            await _context.SaveChangesAsync();
            return review;
        }
    }
}
