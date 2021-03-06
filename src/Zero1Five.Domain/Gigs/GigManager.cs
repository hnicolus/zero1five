using System;
using System.Threading.Tasks;
using Volo.Abp.Domain.Services;

namespace Zero1Five.Gigs
{
    public class GigManager : DomainService, IGigManager
    {
        private readonly IGigRepository _gigRepository;
        public GigManager(IGigRepository gigRepository)
        {
            _gigRepository = gigRepository;
        }

        public Task<Gig> CreateAsync(string title, Guid categoryId,string coverImage, string description)
        {
            var newGig = Gig.Create(GuidGenerator.Create(), categoryId,title, coverImage, description);
            return _gigRepository.InsertAsync(newGig, true);
        }
        public Task<Gig> ChangeCoverImageAsync(Gig gig, string coverImage)
        {
            gig.SetCover(coverImage);
            return Task.FromResult(gig);
        }
    }
}