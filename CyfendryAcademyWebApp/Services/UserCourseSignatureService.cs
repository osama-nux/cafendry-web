using CyfendryAcademyWebApp.Data;
using CyfendryAcademyWebApp.Models;
using CyfendryAcademyWebApp.Repositories;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace CyfendryAcademyWebApp.Services
{
    public class UserCourseSignatureService
    {
        private IUserCourseSignatureRespository _userCourseSignatureRespository;

        public UserCourseSignatureService()
        {
            admin_cyfendrydbEntities admin_CyfendrydbEntities = new admin_cyfendrydbEntities();
            _userCourseSignatureRespository = new UserCourseSignatureRespository(admin_CyfendrydbEntities);
        }

        public async Task<GenericResponseModel<UserCourseSignature>> AddUserCourseSignatureAsync(long userCourseId, string serverPath, HttpPostedFileBase file)
        {
            if (file != null && file.ContentLength > 0)
            {
                string _FileName = Path.GetFileName(userCourseId + file.FileName);
                string _path = Path.Combine(serverPath, _FileName);
                file.SaveAs(_path);

                UserCourseSignature userCourseSignature = new UserCourseSignature { UserCourseId = userCourseId, SignaturePath = _path };

                if (await _userCourseSignatureRespository.CreateAsync(userCourseSignature))
                {
                    return new GenericResponseModel<UserCourseSignature> { model = userCourseSignature, status = true, successMessage = "signature successfully added." };
                }
                else
                {
                    return new GenericResponseModel<UserCourseSignature> { errorMessage = "signature creation failed!", status = false, userMessage = "something went wrong! please try again later." };
                }
            }
            else
            {
                return new GenericResponseModel<UserCourseSignature> { errorMessage = "no file found!", status = false, userMessage = "file could not save please try again .." };
            }
        }
    }
}