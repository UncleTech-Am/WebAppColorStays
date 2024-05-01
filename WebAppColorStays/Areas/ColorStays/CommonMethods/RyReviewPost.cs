using LibCommon.Service;
using UncleTech.Encryption;
using WebAppColorStays.Models.ViewModel;

namespace WebAppColorStays.CommonMethod
{
    public class RyReviewPost
    {
        public CsReviewPost GetReviewQuestions(CsReviewQuestion data)
        {
            CsReviewPost reviewPost = new CsReviewPost();
            reviewPost.QuestionId = data.Id;
            reviewPost.QuestionName = data.Name;
            reviewPost.Fk_ReviewFor_Name= data.Fk_ReviewFor_Name;
            reviewPost.IsRating = data.IsRating;
            reviewPost.IsBool = data.IsBool;
            reviewPost.IsText = data.IsText;    
            return reviewPost;
        }

        public List<CsReview> AddReview(CsReviewPost csReviewPost)
        {
            //var CompID = Process.Decrypt(Base64UrlEncoder.Decode(Request.Cookies["CompanyID"]));
            List<CsReview> list = new List<CsReview>();
            foreach (var item in csReviewPost.QuestionList)
            {
                var review = new CsReview();
                review.Id = Guid.NewGuid().ToString();
                review.Fk_ReviewFor_Name = Process.Decrypt(Base64UrlEncoder.Decode(item.Fk_ReviewFor_Name));
                review.Fk_ReviewQuestion_Name = Process.Decrypt(Base64UrlEncoder.Decode(item.QuestionId));
                review.Fk_Country_Name = Process.Decrypt(Base64UrlEncoder.Decode(csReviewPost.Fk_Country_Name));
                review.Text = item.Text;
                review.Rating = item.Rating;
                review.IsBool = item.IsBoolReview;
                review.IsPhotoUploaded = item.IsPhotoUploaded;
                review.IsRejected = true;
                //review.CompId = CompID;
                list.Add(review);
            }
            return list;
        }


    }

}
