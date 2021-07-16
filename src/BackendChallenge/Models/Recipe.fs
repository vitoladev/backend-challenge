module BackendChallenge.Models.Recipe

open AccidentalFish.FSharp.Validation

type Ingredient =
    { food: string
      quantity: int
      unit: string }

type Equipment = { tool: string; quantity: int }

[<CLIMutable>]
type Recipe =
    { id: string
      name: string
      description: string
      ingredients: Ingredient list
      equipments: Equipment list }

type CreateRecipe = Recipe -> Async<Recipe>
