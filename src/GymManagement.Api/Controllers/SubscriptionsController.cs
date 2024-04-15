using GymManagement.Application.Subscriptions.Commands.CreateSubscription;
using GymManagement.Application.Subscriptions.Queries.GetSubscription;
using GymManagement.Contracts.Subscriptions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using DomainSubscriptionType = GymManagement.Domain.Subscriptions.SubscriptionType;

namespace GymManagement.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SubscriptionsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public SubscriptionsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> CreateSubscription(CreateSubscriptionRequest request)
        {

            //if (DomainSubscriptionType.TryFromName(
            //    request.subscriptionType.ToString(),
            //    out var subscriptionType))
            //{
            //    return Problem(
            //        statusCode: StatusCodes.Status400BadRequest,
            //        detail: "Invalid subscription type.");
            //}

            var subscriptionType = DomainSubscriptionType.FromName(request.subscriptionType.ToString());

            var command = new CreateSubscriptionCommand(
                subscriptionType, 
                request.adminId);

            var createSubscriptionResult = await _mediator.Send(command);

            //The Match method will allow you to return a list of errors instead of the first error
            return createSubscriptionResult.MatchFirst(
                subscription => Ok(new SubscriptionResponse(subscription.Id, request.subscriptionType)),
                error => Problem());


            //if (createSubscriptionResult.IsError) 
            //{
            //    return Problem();
            //}

            //var response = new SubscriptionResponse(
            //    createSubscriptionResult.Value,
            //    request.subscriptionType);

            //return Ok(response);
        }

        [HttpGet("{subscriptionId:guid}")]
        public async Task<IActionResult> GetSubscription(Guid subscriptionId)
        {
            var query = new GetSubscriptionQuery(subscriptionId);

            var getSubscriptionResult = await _mediator.Send(query);

            return getSubscriptionResult.MatchFirst(
                subscription => Ok(new SubscriptionResponse(
                    subscription.Id,
                    Enum.Parse<SubscriptionType>(subscription.SubscriptionType.Name))),
                error => Problem());
        }
    }
}
