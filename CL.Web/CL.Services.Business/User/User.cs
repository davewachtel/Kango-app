using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CL.Services.Data.Repository;
using System.Security.Claims;
using CL.Services.Contracts.Responses;
using CL.Services.Contracts;

namespace CL.Services.Business.User
{
    public class User : Contracts.IUser
    {
        Contracts.IUser _User { get; set; }

        public User(Contracts.IUser user)
        {
            this._User = user;
        }

        public static IUserStore<Contracts.User> GetStore()
        {
            LoginRepository repo = new LoginRepository();
            return repo.GetUserStore();
        }

        

        public static IPagedResponse<IInboxMessage> GetInboxByUserId(String userId, int page, int pageSize)
        {
            if (String.IsNullOrWhiteSpace(userId))
                throw new ArgumentNullException("username");

            UserRepository rep = new UserRepository();
            return rep.GetInboxByUserId(userId, page, pageSize);
        }

        public static IPutResponse MarkMessageAsReadOrUnRead(String userId, IInboxMessage message)
        {
            if (String.IsNullOrWhiteSpace(userId))
                throw new ArgumentNullException("userId");

            UserRepository rep = new UserRepository();
            return rep.MarkMessageAsReadOrUnRead(userId, message.MessageId, message.IsRead);
        }

        public static Dictionary<String, bool> CheckNumbers(String[] numbers)
        {
            if (numbers.Length == 0)
                throw new ArgumentNullException("numbers");

            UserRepository rep = new UserRepository();
            return rep.CheckNumbers(numbers);
        }

        public static int SendInboxMessages(String fromUserId, IShare shareMessage)
        {
            UserRepository rep = new UserRepository();
            return rep.SendInboxMessages(fromUserId, shareMessage);
        }

        public static void RemoveViewsByUserId(String userId)
        {
            if (String.IsNullOrWhiteSpace(userId))
                throw new ArgumentNullException("userId");

            UserRepository rep = new UserRepository();
            rep.RemoveViewsByUserId(userId);

        }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager manager, string authenticationType)
        {

            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var user = ToUser(this._User);
            var userIdentity = await manager.CreateIdentityAsync(user, authenticationType);
            // Add custom user claims here
            return userIdentity;
        }

        private Contracts.User ToUser(Contracts.IUser user)
        {
            return new Contracts.User()
            {
                Id = user.Id,
                UserName = user.UserName,
                PasswordHash = user.PasswordHash,
                SecurityStamp = user.SecurityStamp,
                PhoneNumber = user.PhoneNumber,
                notify_me = user.notify_me,
                device_id = user.device_id
            };
        }

        public string PasswordHash
        {
            get { return this._User.PasswordHash; }
            set { this._User.PasswordHash = value; }
        }

        public string SecurityStamp
        {
            get { return this._User.SecurityStamp; }
            set { this._User.SecurityStamp = value; }
        }

        public string Email
        {
            get { return this._User.Email; }
            set { this._User.Email = value; }
        }

        public bool EmailConfirmed
        {
            get { return this._User.EmailConfirmed; }
            set { this._User.EmailConfirmed = value; }
        }

        public IList<Contracts.IRole> Roles
        {
            get { return this._User.Roles; }
            set { this._User.Roles = value; }
        }

        public string Id
        {
            get { return this._User.Id; }
        }

        public string UserName
        {
            get { return this._User.UserName; }
            set { this._User.UserName = value; }
        }
        public string PhoneNumber
        {
            get { return this._User.PhoneNumber; }
            set { this._User.PhoneNumber = value; }
        }

        public string notify_me
        {
            get { return this._User.notify_me; }
            set { this._User.notify_me = value; }
        }

        public string device_id
        {
            get { return this._User.device_id; }
            set { this._User.device_id = value; }
        }

    }
}
