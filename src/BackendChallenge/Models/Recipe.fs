module BackendChallenge.Models.Recipe

type Ingredient =
    { id: string
      food: string
      quantity: string
      unit: string }

type Equipment = { tool: string; quantity: int }

[<CLIMutable>]
type RecipeCollection =
    { id: string
      name: string
      description: string
      ingredientsRef: ResizeArray<string>
      equipments: ResizeArray<Equipment> }

type Recipe =
    { id: string
      name: string
      description: string
      ingredients: Ingredient list
      equipments: Equipment list }

type CreateRecipe = Recipe -> Async<Recipe>
