using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Blazorise;
using Blazorise.DataGrid;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Volo.Abp.Application.Dtos;
using Zero1Five.Gigs;
using Zero1Five.Permissions;
using Zero1Five.Products;

namespace Zero1Five.Blazor.Pages.Gigs.Manage
{
    public partial class GigsTable
    {
        [Inject]
        public IGigAppService GigAppService { get; set; }
        private IReadOnlyList<GigDto> GigList { get; set; }
        
        [Inject]
        public NavigationManager NavigationManager { get; set; }
        private int PageSize { get; } = LimitedResultRequestDto.DefaultMaxResultCount;
        private int CurrentPage { get; set; }
        private string CurrentSorting { get; set; }
        private int TotalCount { get; set; }

        private bool CanCreateGig { get; set; }
        private bool CanEditGig { get; set; }
        private bool CanDeleteGig { get; set; }
        private bool CanPublish { get; set; }
        private bool ShowModal { get; set; }

        protected override async Task OnInitializedAsync()
        {
            await SetPermissionsAsync();
            await GetProductsAsync();
        }

        private async Task SetPermissionsAsync()
        {
            CanCreateGig = await AuthorizationService.IsGrantedAsync(Zero1FivePermissions.Gigs.Create);

            CanPublish = await AuthorizationService.IsGrantedAnyAsync(Zero1FivePermissions.Gigs.Publish);

            CanEditGig = await AuthorizationService
                .IsGrantedAsync(Zero1FivePermissions.Gigs.Edit);

            CanDeleteGig = await AuthorizationService
                .IsGrantedAsync(Zero1FivePermissions.Gigs.Delete);
        }

        private void HandleProductSubmitted(CreateUpdateProductDto product)
        {
            GetProductsAsync();
        }
        private async Task GetProductsAsync()
        {
            var result = await GigAppService.GetListAsync(
                new GetProductListDto
                {
                    MaxResultCount = PageSize,
                    SkipCount = CurrentPage * PageSize,
                    Sorting = CurrentSorting
                }
            );

            GigList = result.Items;
            TotalCount = (int)result.TotalCount;
        }

        private async Task OnDataGridReadAsync(DataGridReadDataEventArgs<GigDto> e)
        {
            CurrentSorting = e.Columns
                .Where(c => c.Direction != SortDirection.None)
                .Select(c => c.Field + (c.Direction == SortDirection.Descending ? " DESC" : ""))
                .JoinAsString(",");
            CurrentPage = e.Page - 1;

            await GetProductsAsync();
            await InvokeAsync(StateHasChanged);
        }

        private async Task HandlePublish(GigDto gig)
        {
            Guid id = Guid.Empty;
            // if (gig.IsPublished)
            //     id = await GigAppService.UnPublishAsync(gig.Id);
            // else
            //     id = await GigAppService.PublishAsync(gig.Id);
            //
            // if (id != Guid.Empty)
            // {
            //     var message = !gig.IsPublished ? "Published " : "UnPublished";
            //     await Message.Success($"Product successfully {message}");
            // }
            // else
            // {
            //     var message = gig.IsPublished ? "Published " : "UnPublished";
            //     await Message.Error("Failed to " + message);
            // }
            await GetProductsAsync();

        }

        private void OpenGigForm(GigDto gig = null)
        {
            var id = gig?.Id ?? Guid.Empty;
            NavigationManager.NavigateTo("manage/gigs/"+id);
        }
        private async Task DeleteProductAsync(GigDto gig)
        {
            var confirmMessage = L["GigDeletionConfirmationMessage", gig.Title];
            
            if (!await Message.Confirm(confirmMessage)) return;

            await GigAppService.DeleteAsync(gig.Id);
            await GetProductsAsync();
        }
    }
}