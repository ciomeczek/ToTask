using AutoMapper;
using BCrypt.Net;
using Microsoft.AspNetCore.Authorization;
using ToTask.Data;
using ToTask.DTOs;
using ToTask.Models;

namespace ToTask.Services
{
    public class UserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public UserService(IUserRepository userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<UserDTO>> GetAllUsers()
        {
            IEnumerable<User> users = await _userRepository.GetAll();
            return _mapper.Map<IEnumerable<UserDTO>>(users);
        }

        public async Task<UserDTO> GetUserById(int id)
        {
            User user = await _userRepository.GetById(id);
            return _mapper.Map<UserDTO>(user);
        }

        public async Task<User> AddUser(CreateUserDTO userDTO)
        {
            // Generate a salt and hash the password using bcrypt
            string salt = BCrypt.Net.BCrypt.GenerateSalt();
            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(userDTO.Password, salt);

            // Create a new user object with the hashed password
            User user = _mapper.Map<User>(userDTO);
            user.Password = hashedPassword;

            User newUser = await _userRepository.Add(user);
            return newUser;
        }

        public async Task<UserDTO?> UpdateUser(CreateUserDTO userDTO)
        {
            User existingUser = await _userRepository.GetById(userDTO.Id);

            if (existingUser == null)
            {
                return null;
            }

            // Generate a salt and hash the password using bcrypt
            string salt = BCrypt.Net.BCrypt.GenerateSalt();
            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(userDTO.Password, salt);

            // Update the existing user object with the hashed password
            existingUser.Password = hashedPassword;
            existingUser = await _userRepository.Update(existingUser);

            return _mapper.Map<UserDTO>(existingUser);
        }

        public async Task<bool> DeleteUser(int id)
        {
            return await _userRepository.Delete(id);
        }
    }
}
