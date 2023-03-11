namespace DomainTypes

module BusinessLogic =
  type BusId = BusId of string
  type CustomerId = CustomerId of string
  type Point = { x: float; y: float}
  type Vehicle = {CurrentLoc: Point; Id: string; Name: Option<string>}

