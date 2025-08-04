using System.Collections.ObjectModel;

namespace Models.Tickets;

public static class TicketCategoryDescriptions
{
    public static IReadOnlyCollection<(TicketCategory, string)> Descriptions()
    {
        var descriptions = new List<(TicketCategory, string)>()
        {
            (TicketCategory.IT_a_technická_podpora, "Požadavky související s hardwarem, softwarem, sítěmi, uživatelskou podporou nebo správou systémů."),
            (TicketCategory.Spisová_a_dokumentační_agenda, "Požadavky na evidenci, správu, archivaci a zpracování dokumentů a spisů, včetně jejich elektronické formy."),
            (TicketCategory.Vozový_park, "Požadavky týkající se provozu, údržby, evidence nebo rezervace služebních vozidel."),
            (TicketCategory.Radiokomunikační_technika, "Požadavky spojené s provozem, servisem nebo konfigurací radiostanic, vysílaček a jiných komunikačních zařízení."),
            (TicketCategory.Personální_a_služební_záležitosti, "Požadavky související s personální agendou – např. docházka, dovolená, pracovní pomůcky, služební průkazy apod."),
            (TicketCategory.Správa_budov_a_majetku, "Požadavky na opravy, údržbu, technické vybavení budov, revize, správu kancelářského vybavení a jiného majetku."),
            (TicketCategory.Aplikace_a_specializované_systémy, "Požadavky týkající se vývoje, úprav, přístupu nebo problémů ve specifických informačních systémech nebo aplikacích."),
            (TicketCategory.Urgentní_a_krizové_požadavky, "Naléhavé požadavky vyžadující okamžitý zásah – např. havárie, výpadky, bezpečnostní incidenty."),
            (TicketCategory.Jiné, "Ostatní požadavky, které nelze jednoznačně zařadit do výše uvedených kategorií.")
        };
        return new ReadOnlyCollection<(TicketCategory, string)>(descriptions);
    }

    public static IReadOnlyCollection<(TicketCategory, string)> Icons()
    {
        var icons = new List<(TicketCategory, string)>()
        {
            (TicketCategory.IT_a_technická_podpora, "pc-display"),
            (TicketCategory.Spisová_a_dokumentační_agenda, "file-earmark-text"),
            (TicketCategory.Vozový_park, "car-front"),
            (TicketCategory.Radiokomunikační_technika, "broadcast-pin"),
            (TicketCategory.Personální_a_služební_záležitosti, "person-bounding-box"),
            (TicketCategory.Správa_budov_a_majetku, "building"),
            (TicketCategory.Aplikace_a_specializované_systémy, "window-sidebar"),
            (TicketCategory.Urgentní_a_krizové_požadavky, "stopwatch-fill"),
            (TicketCategory.Jiné, "question-circle")
        };
        return new ReadOnlyCollection<(TicketCategory, string)>(icons);
    }
}
