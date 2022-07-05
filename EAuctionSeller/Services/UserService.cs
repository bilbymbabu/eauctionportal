using EAuctionSeller.Data;
using EAuctionSeller.Interface;
using EAuctionSeller.Repository;
using System.Threading.Tasks;
using EAuctionSeller.Extensions;
using EAuctionSeller.Helpers;
using EAuctionSeller.Enum;

namespace EAuctionSeller.Services
{
    public class UserService:IUser
    {
        public readonly IUserRepository userRepository;
        public UserService(IUserRepository _userRepository)
        {
            userRepository = _userRepository;
        }

        public async Task<User> CreateOrUpdateAsync(User user)
        {
            if(!ArgumentValidations.ValidateFirstName( user.FirstName))
                throw new UserException("Please enter valid first name");
            if (!ArgumentValidations.ValidateLastName(user.LastName))
                throw new UserException("Please enter valid last name");
            if (!ArgumentValidations.ValidatePhoneNumber(user.Phone))
                throw new UserException("Please enter valid phone number");
            if (!ArgumentValidations.IsValidEmail(user.Email))
                throw new UserException("Please enter valid email");
            if (user.UserId!=null)
            {
                var existingUser = await userRepository.GetUserByIDAsync(user.UserId);
                if (existingUser == null)
                    throw new UserNotFounException("User not found");
                var updateduser = await userRepository.CreateOrUpdateAsync(user);
                return updateduser;
            }
            else
            { 
                var newUser= await userRepository.CreateOrUpdateAsync(user);
                newUser.UserType = newUser.UserType == "1" ? UserTypeEnum.Seller.ToString() : UserTypeEnum.Buyer.ToString();
                return newUser;
            }
        }
    }
}
