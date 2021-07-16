module BackendChallenge.Models.Validations.Recipe

open AccidentalFish.FSharp.Validation
open BackendChallenge.Models.Recipe

let validateIngredients =
    createValidatorFor<Ingredient> () {
        validate (fun ingredient -> ingredient.food) [ isNotEmptyOrWhitespace; isNotNull ]
        validate (fun ingredient -> ingredient.quantity) [ isGreaterThan 0 ]
        validate (fun ingredient -> ingredient.unit) [ isNotEmptyOrWhitespace; isNotNull ]
    }

let validateEquipment =
    createValidatorFor<Equipment> () {
        validate (fun equipment -> equipment.tool) [ isNotEmptyOrWhitespace; isNotNull ]
        validate (fun equipment -> equipment.quantity) [ isGreaterThan 0 ]
    }

let validateRecipe =
    createValidatorFor<Recipe> () {
        validate
            (fun recipe -> recipe.name)
            [ isNotEmptyOrWhitespace
              isNotNull
              hasMinLengthOf 3 ]

        validate
            (fun recipe -> recipe.description)
            [ isNotEmptyOrWhitespace
              isNotNull
              hasMinLengthOf 5 ]

        validate
            (fun recipe -> recipe.ingredients)
            [ isNotEmpty
              eachItemWith validateIngredients ]

        validate
            (fun recipe -> recipe.equipments)
            [ isNotEmpty
              eachItemWith validateEquipment ]
    }
