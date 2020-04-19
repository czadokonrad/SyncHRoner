using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SyncHRoner.DAL.Repositories;
using SyncHRoner.Dtos;
using SyncHRoner.Dtos.CreateDtos;
using SyncHRoner.Dtos.UpdateDtos;

namespace SyncHRoner.Controllers
{
    [Route("api/profiles")]
    [ApiController]
    public class ProfileController : ControllerBase
    {
        private readonly IProfileRepository _profileRepository;
        private readonly IMapper _mapper;

        public ProfileController(IProfileRepository profileRepository, IMapper mapper)
        {
            _profileRepository = profileRepository;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllProfiles(CancellationToken cancellationToken)
        {
            return (await _profileRepository
                .GetAllAsync(cancellationToken))
                .Match(Left: (failure) => StatusCode(StatusCodes.Status500InternalServerError),
                       Right: (result) => 
                              result.Match<IActionResult>(
                                     some: (result) => Ok(result.Select(x => _mapper.Map<ProfileDto>(x)).ToList()),
                                     none: () => NotFound()));
                
        }

        [HttpGet("active")]
        public async Task<IActionResult> GetAllActiveProfiles(CancellationToken cancellationToken)
        {
            return (await _profileRepository
             .GetAllWhereAsync(x => x.IsActive == true, cancellationToken))
             .Match(Left: (failure) => StatusCode(StatusCodes.Status500InternalServerError),
                    Right: (result) =>
                           result.Match<IActionResult>(
                                  some: (result) => Ok(result),
                                  none: () => NotFound()));
        }

        [HttpGet("{profileid}")]
        public async Task<IActionResult> GetProfileById([FromRoute]long profileid, CancellationToken cancellationToken)
        {
            return (await _profileRepository
                   .GetByIdAsync(profileid, cancellationToken))
                   .Match(Left: (failure) => StatusCode(StatusCodes.Status500InternalServerError),
                          Right: (result) =>
                          result.Match<IActionResult>(
                                  some: (result) => Ok(_mapper.Map<ProfileDto>(result)),
                                  none: () => NotFound()));
        }

        [HttpPost]
        public async Task<IActionResult> CreateProfile([FromBody]ProfileCreateDto profileDto, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var profile = _mapper.Map<SyncHRoner.Domain.Entities.Profile>(profileDto);

            return (await _profileRepository
                   .SaveAsync(profile, cancellationToken))
                                                        //dummy check, of course in enterprise project there would be more proper validation
                   .Match(Left: (failure) => failure.Error.Contains("duplicated") ? Conflict() : StatusCode(StatusCodes.Status500InternalServerError),
                          Right: (result) => Ok());
        }

        [HttpPut]
        public async Task<IActionResult> UpdateProfile([FromBody]ProfileUpdateDto profileDto, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var profile = _mapper.Map<SyncHRoner.Domain.Entities.Profile>(profileDto);

            return (await _profileRepository
                  .UpdateAsync(profile, cancellationToken))
                  .Match(Left: (failure) => failure.Error.Contains("duplicated") ? Conflict() : StatusCode(StatusCodes.Status500InternalServerError),
                         Right: (result) => Ok());
        }

        [HttpDelete("{profileid}")]
        public async Task<IActionResult> DeleteProfile([FromRoute]long profileid, CancellationToken cancellationToken)
        {
            return (await _profileRepository
                 .DeleteAsync(profileid, cancellationToken))
                 .Match(Left: (failure) => StatusCode(StatusCodes.Status500InternalServerError),
                        Right: (result) => Ok());
        }
    }
}