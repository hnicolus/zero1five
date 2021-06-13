using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Npgsql.EntityFrameworkCore.PostgreSQL.Update.Internal;
using Shouldly;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Repositories;
using Xunit;
using Zero1Five.Common;
using Zero1Five.TestBase;

namespace Zero1Five.Gigs
{
    public sealed class GigAppServiceTests:Zero1FiveApplicationTestBase
        {
            private readonly IGigAppService _gigAppService;
            private readonly IGigRepository _gigRepository;
            
            public GigAppServiceTests()
            {
                _gigRepository = GetRequiredService<IGigRepository>();
                _gigAppService = GetRequiredService<IGigAppService>();
            }

            [Fact(Skip = "Blob services causing tests to fail")]
            public async Task CreateAsync_Should_CreateGig()
            {
                var input = new CreateUpdateGigDto()
                {
                    Title = "Coolest Gig",
                    Description = "This is a cool new gig",
                    Cover =  new SaveFileDto()
                    {
                        FileName = "coolgImage.jpg",
                        Content =  new Byte[]{12,34,45,45}
                    }
                };

                var result =await _gigAppService.CreateAsync(input);
                
                result.Id.ShouldNotBe(Guid.Empty);
                result.Title.ShouldBe(input.Title);
                result.Description.ShouldBe(input.Description);
            }

            [Fact(Skip = "Blob services causing tests to fail")]
            public async Task GetAsync_Should_GetGigOfGivenId()
            {
                var gigId = Guid.Parse(Zero1FiveTestData.GigId);
                var result =await _gigAppService.GetAsync(gigId);
                
                result.ShouldNotBeNull();
                result.Id.ShouldBe(gigId);
            }

            [Fact(Skip = "Blob services causing tests to fail")]
            public async Task GetListAsync_ShouldGetGigList()
            {
                var result = await _gigAppService.GetListAsync(new PagedSortableAndFilterableRequestDto());
                
                result.Items.Count.ShouldBeGreaterThanOrEqualTo(0);
            }
            [Fact(Skip = "Blob services causing tests to fail")]
            public async Task UpdateAsync_ShouldUpdateGig()
            {
                var gig =await WithUnitOfWorkAsync<Gig>(() => _gigRepository.FirstOrDefaultAsync());
                
                var input = new CreateUpdateGigDto()
                {
                    Title = "newUpdated",
                    Description = "this is Some new Description here buddy"
                };

                var result =await _gigAppService.UpdateAsync(gig.Id, input);

                result.ShouldNotBeNull();
                result.Id.ShouldBe(gig.Id);
                result.Title.ShouldBe(input.Title);
                result.Description.ShouldBe(input.Description);
            }
            [Fact(Skip = "Blob services causing tests to fail")]
            public async Task DeleteAsync_Should_DeleteGig_Async()
            {
                //Given
                var gig = (await _gigAppService.GetListAsync(new PagedAndSortedResultRequestDto())).Items[0];
                //When
                await _gigAppService.DeleteAsync(gig.Id);
                var results = (await _gigAppService.GetListAsync(new PagedAndSortedResultRequestDto())).Items;
                //Then
                results.ShouldNotContain(gig);
            }

            [Fact(Skip = "Blob services causing tests to fail")]
            public async Task PublishAsync_SHouldPublishGig()
            {
                //given 
                var gig = (await _gigAppService.GetListAsync(new PagedAndSortedResultRequestDto())).Items.First();
                //when publish
               var result =  await _gigAppService.PublishAsync(gig.Id);
               
               result.Id.ShouldBe(gig.Id);
               result.IsPublished.ShouldBe(true);
            }
            
            [Fact(Skip = "Blob services causing tests to fail")]
            public async Task UnpublishAsync_SHouldUnpublishGig()
            {
                //given 
                var gig = (await _gigAppService.GetListAsync(new PagedAndSortedResultRequestDto())).Items.First();
                //when Unpublish requested
                var result =  await _gigAppService.UnpublishAsync(gig.Id);
               //then
                result.Id.ShouldBe(gig.Id);
                result.IsPublished.ShouldBe(false);
            }
        }
}