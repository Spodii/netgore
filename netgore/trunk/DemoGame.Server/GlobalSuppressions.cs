// This file is used by Code Analysis to maintain SuppressMessage 
// attributes that are applied to this project.
// Project-level suppressions either have no target or are given 
// a specific target and scoped to a namespace, type, member, etc.
//
// To add a suppression to this file, right-click the message in the 
// Error List, point to "Suppress Message(s)", and click 
// "In Project Suppression File".
// You do not need to add suppressions to this file manually.

using System.Diagnostics.CodeAnalysis;
using System.Linq;

[assembly: SuppressMessage("Microsoft.Usage", "CA2232:MarkWindowsFormsEntryPointsWithStaThread")]
[assembly:
    SuppressMessage("Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields", Scope = "member",
        Target = "DemoGame.Server.Queries.CreateAccountQuery+QueryArgs.#AccountID")]
[assembly:
    SuppressMessage("Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields", Scope = "member",
        Target = "DemoGame.Server.Queries.CreateAccountQuery+QueryArgs.#Email")]
[assembly:
    SuppressMessage("Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields", Scope = "member",
        Target = "DemoGame.Server.Queries.CreateAccountQuery+QueryArgs.#IP")]
[assembly:
    SuppressMessage("Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields", Scope = "member",
        Target = "DemoGame.Server.Queries.CreateAccountQuery+QueryArgs.#Name")]
[assembly:
    SuppressMessage("Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields", Scope = "member",
        Target = "DemoGame.Server.Queries.CreateAccountQuery+QueryArgs.#Password")]
[assembly:
    SuppressMessage("Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields", Scope = "member",
        Target = "DemoGame.Server.Queries.CreateUserOnAccountQuery+QueryArgs.#AccountName")]
[assembly:
    SuppressMessage("Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields", Scope = "member",
        Target = "DemoGame.Server.Queries.CreateUserOnAccountQuery+QueryArgs.#CharacterID")]
[assembly:
    SuppressMessage("Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields", Scope = "member",
        Target = "DemoGame.Server.Queries.CreateUserOnAccountQuery+QueryArgs.#UserName")]
[assembly:
    SuppressMessage("Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields", Scope = "member",
        Target = "DemoGame.Server.Queries.DeleteCharacterQuestStatusKillsQuery+QueryArgs.#CharacterID")]
[assembly:
    SuppressMessage("Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields", Scope = "member",
        Target = "DemoGame.Server.Queries.DeleteCharacterQuestStatusKillsQuery+QueryArgs.#QuestID")]
[assembly:
    SuppressMessage("Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields", Scope = "member",
        Target = "DemoGame.Server.Queries.DeleteCharacterQuestStatusQuery+QueryArgs.#CharacterID")]
[assembly:
    SuppressMessage("Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields", Scope = "member",
        Target = "DemoGame.Server.Queries.DeleteCharacterQuestStatusQuery+QueryArgs.#QuestID")]
[assembly:
    SuppressMessage("Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields", Scope = "member",
        Target = "DemoGame.Server.Queries.InsertAccountIPQuery+QueryArgs.#AccountID")]
[assembly:
    SuppressMessage("Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields", Scope = "member",
        Target = "DemoGame.Server.Queries.InsertAccountIPQuery+QueryArgs.#IP")]
[assembly:
    SuppressMessage("Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields", Scope = "member",
        Target = "DemoGame.Server.Queries.InsertCharacterQuestStatusStartQuery+QueryArgs.#CharacterID")]
[assembly:
    SuppressMessage("Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields", Scope = "member",
        Target = "DemoGame.Server.Queries.InsertCharacterQuestStatusStartQuery+QueryArgs.#QuestID")]
[assembly:
    SuppressMessage("Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields", Scope = "member",
        Target = "DemoGame.Server.Queries.InsertGuildEventQuery+QueryArgs.#Arg0")]
[assembly:
    SuppressMessage("Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields", Scope = "member",
        Target = "DemoGame.Server.Queries.InsertGuildEventQuery+QueryArgs.#Arg1")]
[assembly:
    SuppressMessage("Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields", Scope = "member",
        Target = "DemoGame.Server.Queries.InsertGuildEventQuery+QueryArgs.#Arg2")]
[assembly:
    SuppressMessage("Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields", Scope = "member",
        Target = "DemoGame.Server.Queries.InsertGuildEventQuery+QueryArgs.#CharacterID")]
[assembly:
    SuppressMessage("Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields", Scope = "member",
        Target = "DemoGame.Server.Queries.InsertGuildEventQuery+QueryArgs.#EventID")]
[assembly:
    SuppressMessage("Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields", Scope = "member",
        Target = "DemoGame.Server.Queries.InsertGuildEventQuery+QueryArgs.#GuildID")]
[assembly:
    SuppressMessage("Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields", Scope = "member",
        Target = "DemoGame.Server.Queries.InsertGuildEventQuery+QueryArgs.#TargetID")]
[assembly:
    SuppressMessage("Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields", Scope = "member",
        Target = "DemoGame.Server.Queries.InsertGuildMemberQuery+QueryArgs.#CharacterID")]
[assembly:
    SuppressMessage("Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields", Scope = "member",
        Target = "DemoGame.Server.Queries.InsertGuildMemberQuery+QueryArgs.#GuildID")]
[assembly:
    SuppressMessage("Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields", Scope = "member",
        Target = "DemoGame.Server.Queries.InsertGuildMemberQuery+QueryArgs.#Rank")]
[assembly:
    SuppressMessage("Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields", Scope = "member",
        Target = "DemoGame.Server.Queries.PeerTradingInsertItemQuery+QueryArgs.#CharacterID")]
[assembly:
    SuppressMessage("Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields", Scope = "member",
        Target = "DemoGame.Server.Queries.PeerTradingInsertItemQuery+QueryArgs.#ItemID")]
[assembly:
    SuppressMessage("Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields", Scope = "member",
        Target = "DemoGame.Server.Queries.TrySetAccountIPIfNullQuery+QueryArgs.#AccountID")]
[assembly:
    SuppressMessage("Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields", Scope = "member",
        Target = "DemoGame.Server.Queries.TrySetAccountIPIfNullQuery+QueryArgs.#IP")]
[assembly:
    SuppressMessage("Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields", Scope = "member",
        Target = "DemoGame.Server.Queries.UpdateCharacterQuestStatusFinishedQuery+QueryArgs.#CharacterID")]
[assembly:
    SuppressMessage("Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields", Scope = "member",
        Target = "DemoGame.Server.Queries.UpdateCharacterQuestStatusFinishedQuery+QueryArgs.#QuestID")]
[assembly:
    SuppressMessage("Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields", Scope = "member",
        Target = "DemoGame.Server.Queries.UpdateGuildNameQuery+QueryArgs.#ID")]
[assembly:
    SuppressMessage("Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields", Scope = "member",
        Target = "DemoGame.Server.Queries.UpdateGuildNameQuery+QueryArgs.#Value")]
[assembly:
    SuppressMessage("Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields", Scope = "member",
        Target = "DemoGame.Server.Queries.UpdateGuildTagQuery+QueryArgs.#ID")]
[assembly:
    SuppressMessage("Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields", Scope = "member",
        Target = "DemoGame.Server.Queries.UpdateGuildTagQuery+QueryArgs.#Value")]
[assembly:
    SuppressMessage("Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields", Scope = "member",
        Target = "DemoGame.Server.Queries.UpdateItemFieldQuery+QueryArgs.#ItemID")]
[assembly:
    SuppressMessage("Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields", Scope = "member",
        Target = "DemoGame.Server.Queries.UpdateItemFieldQuery+QueryArgs.#Value")]
[assembly:
    SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters", Scope = "member",
        Target =
            "DemoGame.Server.DbObjs.AccountBanTableDbExtensions.#TryReadValues(DemoGame.Server.DbObjs.AccountBanTable,System.Data.IDataReader)"
        )]
[assembly:
    SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters", Scope = "member",
        Target =
            "DemoGame.Server.DbObjs.AccountCharacterTableDbExtensions.#ReadValues(DemoGame.Server.DbObjs.AccountCharacterTable,System.Data.IDataReader)"
        )]
[assembly:
    SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters", Scope = "member",
        Target =
            "DemoGame.Server.DbObjs.AccountCharacterTableDbExtensions.#TryReadValues(DemoGame.Server.DbObjs.AccountCharacterTable,System.Data.IDataReader)"
        )]
[assembly:
    SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters", Scope = "member",
        Target =
            "DemoGame.Server.DbObjs.ActiveTradeCashTableDbExtensions.#ReadValues(DemoGame.Server.DbObjs.ActiveTradeCashTable,System.Data.IDataReader)"
        )]
[assembly:
    SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters", Scope = "member",
        Target =
            "DemoGame.Server.DbObjs.ActiveTradeCashTableDbExtensions.#TryReadValues(DemoGame.Server.DbObjs.ActiveTradeCashTable,System.Data.IDataReader)"
        )]
[assembly:
    SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters", Scope = "member",
        Target =
            "DemoGame.Server.DbObjs.ActiveTradeItemTableDbExtensions.#ReadValues(DemoGame.Server.DbObjs.ActiveTradeItemTable,System.Data.IDataReader)"
        )]
[assembly:
    SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters", Scope = "member",
        Target =
            "DemoGame.Server.DbObjs.ActiveTradeItemTableDbExtensions.#TryReadValues(DemoGame.Server.DbObjs.ActiveTradeItemTable,System.Data.IDataReader)"
        )]
[assembly:
    SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters", Scope = "member",
        Target =
            "DemoGame.Server.DbObjs.AllianceAttackableTableDbExtensions.#ReadValues(DemoGame.Server.DbObjs.AllianceAttackableTable,System.Data.IDataReader)"
        )]
[assembly:
    SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters", Scope = "member",
        Target =
            "DemoGame.Server.DbObjs.AllianceAttackableTableDbExtensions.#TryReadValues(DemoGame.Server.DbObjs.AllianceAttackableTable,System.Data.IDataReader)"
        )]
[assembly:
    SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters", Scope = "member",
        Target =
            "DemoGame.Server.DbObjs.AllianceHostileTableDbExtensions.#ReadValues(DemoGame.Server.DbObjs.AllianceHostileTable,System.Data.IDataReader)"
        )]
[assembly:
    SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters", Scope = "member",
        Target =
            "DemoGame.Server.DbObjs.AllianceHostileTableDbExtensions.#TryReadValues(DemoGame.Server.DbObjs.AllianceHostileTable,System.Data.IDataReader)"
        )]
[assembly:
    SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters", Scope = "member",
        Target =
            "DemoGame.Server.DbObjs.AllianceTableDbExtensions.#ReadValues(DemoGame.Server.DbObjs.AllianceTable,System.Data.IDataReader)"
        )]
[assembly:
    SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters", Scope = "member",
        Target =
            "DemoGame.Server.DbObjs.AllianceTableDbExtensions.#TryReadValues(DemoGame.Server.DbObjs.AllianceTable,System.Data.IDataReader)"
        )]
[assembly:
    SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters", Scope = "member",
        Target =
            "DemoGame.Server.DbObjs.CharacterEquippedTableDbExtensions.#ReadValues(DemoGame.Server.DbObjs.CharacterEquippedTable,System.Data.IDataReader)"
        )]
[assembly:
    SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters", Scope = "member",
        Target =
            "DemoGame.Server.DbObjs.CharacterEquippedTableDbExtensions.#TryReadValues(DemoGame.Server.DbObjs.CharacterEquippedTable,System.Data.IDataReader)"
        )]
[assembly:
    SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters", Scope = "member",
        Target =
            "DemoGame.Server.DbObjs.CharacterInventoryTableDbExtensions.#ReadValues(DemoGame.Server.DbObjs.CharacterInventoryTable,System.Data.IDataReader)"
        )]
[assembly:
    SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters", Scope = "member",
        Target =
            "DemoGame.Server.DbObjs.CharacterInventoryTableDbExtensions.#TryReadValues(DemoGame.Server.DbObjs.CharacterInventoryTable,System.Data.IDataReader)"
        )]
[assembly:
    SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters", Scope = "member",
        Target = "DemoGame.Server.Queries.IDataReaderExtensions.#GetEquipmentSlot(System.Data.IDataReader,System.Int32)")]
[assembly:
    SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters", Scope = "member",
        Target = "DemoGame.Server.Queries.IDataReaderExtensions.#GetItemType(System.Data.IDataReader,System.Int32)")]
[assembly:
    SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters", Scope = "member",
        Target =
            "DemoGame.Server.DbObjs.ServerTimeTableDbExtensions.#ReadValues(DemoGame.Server.DbObjs.ServerTimeTable,System.Data.IDataReader)"
        )]
[assembly:
    SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters", Scope = "member",
        Target =
            "DemoGame.Server.DbObjs.ServerTimeTableDbExtensions.#TryReadValues(DemoGame.Server.DbObjs.ServerTimeTable,System.Data.IDataReader)"
        )]
[assembly:
    SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters", Scope = "member",
        Target =
            "DemoGame.Server.DbObjs.AccountBanTableDbExtensions.#ReadValues(DemoGame.Server.DbObjs.AccountBanTable,System.Data.IDataReader)"
        )]