using System;
using TreasuryDepartment.Services;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using TreasuryDepartment.Models;
using Microsoft.AspNetCore.Mvc;


namespace TreasuryDepartment.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class InvitesController : ControllerBase
	{
		private readonly InviteService _inviteService;
		private readonly UserService _userService;
		public InvitesController(InviteService inviteService, UserService userService)
		{
			_inviteService = inviteService;
			_userService = userService;
		}
		private delegate Task<List<Invite>> GetInvitesDelegate(long id);

		private async Task<ActionResult<List<Invite>>> Get(long id, GetInvitesDelegate getInvitesDelegate)
		{
			var user = await _userService.Get(id);
			if (user == null)
			{
				return NotFound();
			}
			var invites = await getInvitesDelegate(user.Id);
			return new OkObjectResult(invites.Select(i => new
			{
				user = getInvitesDelegate == _inviteService.GetReceivedInvites ? i.SenderUser : i.TargetUser,
				status = i.Status
			}));
		}


		[HttpGet("sent/{id}")]
		public async Task<ActionResult<List<Invite>>> GetSent(long id) =>
			await Get(id, _inviteService.GetSentInvites);


		[HttpGet("received/{id}")]
		public async Task<ActionResult<List<Invite>>> GetReceived(long id) =>
			await Get(id, _inviteService.GetReceivedInvites);


		[HttpPost("from/{fromId}/to/{toId}")]
		public async Task<ActionResult<Invite>> Post(long fromId, long toId)
		{
			var fromUser = await _userService.Get(fromId);
			var toUser = await _userService.Get(toId);
			if (fromUser == null || toUser == null)
				return NotFound();

			var invite = await _inviteService.Get(fromId, toId);
			if (invite != null)
				return BadRequest();

			return await _inviteService.Create(new Invite(fromUser.Id, toUser.Id));
		}

		[HttpPost("accept/from/{fromId}/to/{toId}")]
		public async Task<ActionResult> Accept(long fromId, long toId)
		{
			var fromUser = await _userService.Get(fromId);
			var toUser = await _userService.Get(toId);
			var invite = await _inviteService.Get(fromId, toId);
			if (fromUser == null || toUser == null || invite == null)
				return NotFound();

			if (invite.Status != InviteStatus.Created)
				return BadRequest();

			await _inviteService.Accept(invite);
			return NoContent();
		}

		[HttpPost("decline/from/{fromId}/to/{toId}")]
		public async Task<ActionResult> Decline(long fromId, long toId)
		{
			var fromUser = await _userService.Get(fromId);
			var toUser = await _userService.Get(toId);
			var invite = await _inviteService.Get(fromId, toId);
			if (fromUser == null || toUser == null || invite == null)
				return NotFound();

			if (invite.Status != InviteStatus.Created)
				return BadRequest();

			await _inviteService.Decline(invite);
			return NoContent();
		}

		[HttpDelete("from/{fromId}/to/{toId}")]
		public async Task<ActionResult> Delete(long fromId, long toId)
		{
			var fromUser = await _userService.Get(fromId);
			var toUser = await _userService.Get(toId);
			var invite = await _inviteService.Get(fromId, toId);
			if (fromUser == null || toUser == null || invite == null)
				return NotFound();

			await _inviteService.Delete(invite);
			return NoContent();
		}
	}
}
