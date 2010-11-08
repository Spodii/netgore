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
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields", Scope = "member", Target = "DemoGame.Server.Queries.CreateAccountQuery+QueryArgs.#AccountID")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields", Scope = "member", Target = "DemoGame.Server.Queries.CreateAccountQuery+QueryArgs.#Email")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields", Scope = "member", Target = "DemoGame.Server.Queries.CreateAccountQuery+QueryArgs.#IP")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields", Scope = "member", Target = "DemoGame.Server.Queries.CreateAccountQuery+QueryArgs.#Name")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields", Scope = "member", Target = "DemoGame.Server.Queries.CreateAccountQuery+QueryArgs.#Password")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields", Scope = "member", Target = "DemoGame.Server.Queries.CreateUserOnAccountQuery+QueryArgs.#AccountName")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields", Scope = "member", Target = "DemoGame.Server.Queries.CreateUserOnAccountQuery+QueryArgs.#CharacterID")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields", Scope = "member", Target = "DemoGame.Server.Queries.CreateUserOnAccountQuery+QueryArgs.#UserName")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields", Scope = "member", Target = "DemoGame.Server.Queries.DeleteCharacterQuestStatusKillsQuery+QueryArgs.#CharacterID")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields", Scope = "member", Target = "DemoGame.Server.Queries.DeleteCharacterQuestStatusKillsQuery+QueryArgs.#QuestID")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields", Scope = "member", Target = "DemoGame.Server.Queries.DeleteCharacterQuestStatusQuery+QueryArgs.#CharacterID")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields", Scope = "member", Target = "DemoGame.Server.Queries.DeleteCharacterQuestStatusQuery+QueryArgs.#QuestID")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields", Scope = "member", Target = "DemoGame.Server.Queries.InsertAccountIPQuery+QueryArgs.#AccountID")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields", Scope = "member", Target = "DemoGame.Server.Queries.InsertAccountIPQuery+QueryArgs.#IP")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields", Scope = "member", Target = "DemoGame.Server.Queries.InsertCharacterQuestStatusStartQuery+QueryArgs.#CharacterID")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields", Scope = "member", Target = "DemoGame.Server.Queries.InsertCharacterQuestStatusStartQuery+QueryArgs.#QuestID")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields", Scope = "member", Target = "DemoGame.Server.Queries.InsertGuildEventQuery+QueryArgs.#Arg0")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields", Scope = "member", Target = "DemoGame.Server.Queries.InsertGuildEventQuery+QueryArgs.#Arg1")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields", Scope = "member", Target = "DemoGame.Server.Queries.InsertGuildEventQuery+QueryArgs.#Arg2")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields", Scope = "member", Target = "DemoGame.Server.Queries.InsertGuildEventQuery+QueryArgs.#CharacterID")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields", Scope = "member", Target = "DemoGame.Server.Queries.InsertGuildEventQuery+QueryArgs.#EventID")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields", Scope = "member", Target = "DemoGame.Server.Queries.InsertGuildEventQuery+QueryArgs.#GuildID")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields", Scope = "member", Target = "DemoGame.Server.Queries.InsertGuildEventQuery+QueryArgs.#TargetID")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields", Scope = "member", Target = "DemoGame.Server.Queries.InsertGuildMemberQuery+QueryArgs.#CharacterID")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields", Scope = "member", Target = "DemoGame.Server.Queries.InsertGuildMemberQuery+QueryArgs.#GuildID")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields", Scope = "member", Target = "DemoGame.Server.Queries.InsertGuildMemberQuery+QueryArgs.#Rank")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields", Scope = "member", Target = "DemoGame.Server.Queries.PeerTradingInsertItemQuery+QueryArgs.#CharacterID")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields", Scope = "member", Target = "DemoGame.Server.Queries.PeerTradingInsertItemQuery+QueryArgs.#ItemID")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields", Scope = "member", Target = "DemoGame.Server.Queries.TrySetAccountIPIfNullQuery+QueryArgs.#AccountID")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields", Scope = "member", Target = "DemoGame.Server.Queries.TrySetAccountIPIfNullQuery+QueryArgs.#IP")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields", Scope = "member", Target = "DemoGame.Server.Queries.UpdateCharacterQuestStatusFinishedQuery+QueryArgs.#CharacterID")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields", Scope = "member", Target = "DemoGame.Server.Queries.UpdateCharacterQuestStatusFinishedQuery+QueryArgs.#QuestID")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields", Scope = "member", Target = "DemoGame.Server.Queries.UpdateGuildNameQuery+QueryArgs.#ID")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields", Scope = "member", Target = "DemoGame.Server.Queries.UpdateGuildNameQuery+QueryArgs.#Value")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields", Scope = "member", Target = "DemoGame.Server.Queries.UpdateGuildTagQuery+QueryArgs.#ID")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields", Scope = "member", Target = "DemoGame.Server.Queries.UpdateGuildTagQuery+QueryArgs.#Value")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields", Scope = "member", Target = "DemoGame.Server.Queries.UpdateItemFieldQuery+QueryArgs.#ItemID")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields", Scope = "member", Target = "DemoGame.Server.Queries.UpdateItemFieldQuery+QueryArgs.#Value")]
