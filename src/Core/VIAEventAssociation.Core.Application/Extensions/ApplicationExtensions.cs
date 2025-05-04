using Microsoft.Extensions.DependencyInjection;
using ViaEventAssociation.Core.Application.CommandDispatching;
using ViaEventAssociation.Core.Application.CommandDispatching.Commands.Guest;
using ViaEventAssociation.Core.Application.Features.Commands.Event;
using ViaEventAssociation.Core.Application.Features.Commands.Guest;
using ViaEventAssociation.Core.Application.Features.Event;
using ViaEventAssociation.Core.Application.Features.Guest;
using ViaEventAssociation.Core.QueryContracts.Contracts;
using ViaEventAssociation.Core.QueryContracts.Queries;
using ViaEventAssociation.Infrastructure.EfcQueries.Queries;

namespace ViaEventAssociation.Core.Application.Extensions;

public static class ApplicationExtensions
{
    public static void RegisterCommandHandlers(this IServiceCollection services)
    {
        //events
        services.AddScoped<ICommandHandler<ActivateEventCommand>, ActivateEventHandler>();
        services.AddScoped<ICommandHandler<CreateEventCommand>, CreateEventHandler>();
        services.AddScoped<ICommandHandler<InviteGuestCommand>, InviteGuestHandler>();
        services.AddScoped<ICommandHandler<MakePrivateCommand>, MakePrivateHandler>();
        services.AddScoped<ICommandHandler<MakePublicCommand>, MakePublicHandler>();
        services.AddScoped<ICommandHandler<SetMaxGuestCommand>, SetMaxGuestHandler>();
        services.AddScoped<ICommandHandler<SetReadyEventCommand>, SetReadyEventHandler>();
        services.AddScoped<ICommandHandler<UpdateDescriptionCommand>, UpdateDescriptionHandler>();
        services.AddScoped<ICommandHandler<UpdateTimeIntervalCommand>, UpdateTimeIntervalHandler>();
        services.AddScoped<ICommandHandler<UpdateTitleCommand>, UpdateTitleHandler>();

        //guests
        services.AddScoped<ICommandHandler<AcceptsInvitationCommand>, AcceptsInvitationHandler>();
        services.AddScoped<ICommandHandler<CancelEventParticipationCommand>, CancelEventParticipationHandler>();
        services.AddScoped<ICommandHandler<RegisterGuestCommand>, RegisterGuestHandler>();
        services.AddScoped<ICommandHandler<RejectInvitationCommand>, RejectInvitationHandler>();
        services.AddScoped<ICommandHandler<RequestToJoinCommand>, RequestToJoinHandler>();
    }

    public static void RegisterQueryHandlers(this IServiceCollection services)
    {
        // Ask TROELS if it is okay to do this
        services.AddScoped<IQueryHandler<GuestProfilePage.Query, GuestProfilePage.Answer>, ProfilePageQueryHandler>();
        services.AddScoped<IQueryHandler<UpcomingEventPage.Query, UpcomingEventPage.Answer>, UpcomingEventPageQueryHandler>();
        services.AddScoped<IQueryHandler<ViewSingleEvent.Query, ViewSingleEvent.Answer>, ViewSingleEventQueryHandler>();
        services.AddScoped<IQueryHandler<ViewUnpublishedEvents.Query, ViewUnpublishedEvents.Answer>, ViewUnpublishedEventsQueryHandler>();
    }
}