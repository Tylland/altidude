namespace Altidude.Contracts.Commands
//open Altidude.Contracts.Types;
open Altidude.Messaging
open System

// Customer commands
type CreateProfile = {Id: Guid; Name: string } with interface ICommand
