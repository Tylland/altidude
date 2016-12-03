namespace Altidude.Contracts.Types
open System


type GeoLocation = { Latitude: double; Longitude: double; Altitude: double; }
type TrackPoint = { Location: GeoLocation; Distance: double; Time: DateTime; }
type Track = { Points: TrackPoint[] }

type Place = { Location: GeoLocation; Radius: double; }

//type OrderLine = {ProductId: Guid; ProductName: string; OriginalPrice: int; DiscountedPrice: int; Quantity: int}
//    with override this.ToString() = sprintf "ProdcutName: %s, Price: %d, Discounted: %d, Quantity: %d" this.ProductName this.OriginalPrice this.DiscountedPrice this.Quantity
