using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Erc.Households.BranchOfficeManagment.Core;
using AutoMapper;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.SignalR;
using Erc.Households.Api.UserNotifications;
using System.Threading.Tasks;
using MediatR;
using Erc.Households.Commands;

namespace Erc.Households.Api.Controllers
{
    [Route("api/branch-offices")]
    [ApiController]
    [Authorize]
    public class BranchOfficesController : ErcControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IBranchOfficeService _branchOfficeService;
        readonly IMapper _mapper;
        private readonly IHubContext<UserNotificationHub> _hubContext;
        readonly ConnectedClientsRepository _connectedClientsRepository;

        public BranchOfficesController(IBranchOfficeService branchOfficeService, IMapper mapper, IHubContext<UserNotificationHub> hubContext, ConnectedClientsRepository connectedClientsRepository, IMediator mediator)
        {
            _branchOfficeService = branchOfficeService ?? throw new ArgumentNullException(nameof(branchOfficeService));
            _mapper = mapper;
            _hubContext = hubContext;
            _connectedClientsRepository = connectedClientsRepository;
            _mediator = mediator;
        }

        [HttpGet("")]
        public IActionResult GetAll()
        {
            return Ok(_mapper.Map<IEnumerable<Responses.BranchOffice>>(_branchOfficeService.GetList(UserGroups)).OrderBy(bo => bo.Id < 0 ? 0 : 1).ThenBy(bo => bo.Name));
        }

        [HttpGet("{id}")]
        public IActionResult GetBranchOffice(int id) => Ok(_branchOfficeService.GetOne(id));

        [HttpPut("{id}")]
        public async Task<Unit> Update(Domain.BranchOffice branchOffice)
        {
            return await _mediator.Send(new UpdateBranchOfficeCommand(branchOffice.Id, branchOffice.Name, branchOffice.Address, branchOffice.Iban, branchOffice.BankFullName, branchOffice.ChiefName, branchOffice.BookkeeperName));
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