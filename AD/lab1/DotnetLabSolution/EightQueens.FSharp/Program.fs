/// Generate all X,Y coordinates on the board
/// (initially, all of them are available)
let all =
  [ for x in 0 .. 7 do
    for y in 0 .. 7 do
    yield x, y ]

/// Given available positions on the board, filter
/// out those that are taken by a newly added queen
/// at the position qx, qy
let filterAvailable (qx, qy) available =
  available
  |> List.filter (fun (x, y) ->
    // horizontal & vertical
    x <> qx && y <> qy &&
    // two diagonals
    (x-y) <> (qx-qy) &&
    (7-x-y) <> (7-qx-qy))

/// Generate all solutions. Given already added queens
/// and remaining available positions, we handle 3 cases:
///  1. we have 8 queens - yield their locations
///  2. we have no available places - nothing :(
///  3. we have available place
let rec solve queens available = seq {
  match queens, available with
  | q, _ when List.length q = 8 -> yield queens
  | _, [] -> ()
  | _, a::available ->
      // generate all solutions with queen at 'a'
      yield! solve (a::queens) (filterAvailable a available)
      // generate all solutions with nothing at 'a'
      yield! solve queens available }

/// Nicely render the queen locations
let render items =
  let arr = Array.init 8 (fun _ -> Array.create 8 " . ")
  for x, y in items do arr.[x].[y] <- " x "
  for a in arr do printfn "%s" (String.concat "" a)
  printfn "%s" (String.replicate 24 "-")

// Print all solutions :-)
solve [] all |> Seq.iter render