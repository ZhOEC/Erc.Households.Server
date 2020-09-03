using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Erc.Households.BranchOfficeManagment.Core;
using AutoMapper;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.SignalR;
using Erc.Households.Api.UserNotifications;

namespace Erc.Households.Api.Controllers
{
    [Route("api/branch-offices")]
    [ApiController]
    [Authorize]
    public class BranchOfficesController : ErcControllerBase
    {
        private readonly IBranchOfficeService _branchOfficeService;
        readonly IMapper _mapper;
        private readonly IHubContext<UserNotificationHub> _hubContext;
        readonly ConnectedClientsRepository _connectedClientsRepository;

        public BranchOfficesController(IBranchOfficeService branchOfficeService, IMapper mapper, IHubContext<UserNotificationHub> hubContext, ConnectedClientsRepository connectedClientsRepository)
        {
            _branchOfficeService = branchOfficeService ?? throw new ArgumentNullException(nameof(branchOfficeService));
            _mapper = mapper;
            _hubContext = hubContext;
            _connectedClientsRepository = connectedClientsRepository;
        }

        [HttpGet("")]
        public IActionResult GetAll()
        {
            return Ok(_mapper.Map<IEnumerable<Responses.BranchOffice>>(_branchOfficeService.GetList(UserGroups)).OrderBy(bo=>bo.Name));
        }

        [HttpPost("{id}/periods")]
        public async System.Threading.Tasks.Task<IActionResult> StartNewPeriodAsync(int id)
        {
            _branchOfficeService.StartNewPeriod(id);
            var bo = _branchOfficeService.GetOne(id);
            return await _hubContext.Clients.GroupExcept(bo.StringId, _connectedClientsRepository.GetConnectionId(UserId))
                .SendAsync("ShowUserNotification", new InfoUserNotification { Text = $"Користувач {User.Identity.Name} перевів {bo.Name} на новий період: {bo.CurrentPeriod.Name}", Title = "Новий період", UiModule="branch-office"})
                .ContinueWith(r => Ok());
        }
    }
}