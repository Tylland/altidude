namespace Altidude.Contracts.Events
//open Altidude.Contracts.Types
open Altidude.Messaging
open System

type ProfileCreated = {Id: Guid; Name: string } 
    with interface IEvent with member this.Id with get() = this.Id;