module BackendChallenge.Models.Validations.Recipe

open AccidentalFish.FSharp.Validation
open BackendChallenge.Models.Recipe

let validateIngredient =
    let validateIngredientId =
        createValidatorFor<Ingredient> () { validate (fun i -> i.id) [ isNotEmptyOrWhitespace ] }

    let validateNewIngredientProps =
        createValidatorFor<Ingredient> () {
            validate (fun i -> i.food) [ isNotEmptyOrWhitespace; isNotNull ]
            validate (fun i -> i.quantity) [ isNotEmptyOrWhitespace; isNotNull ]
            validate (fun i -> i.unit) [ isNotEmptyOrWhitespace; isNotNull ]
        }

    createValidatorFor<Ingredient> () {
        validate
            (fun o -> o)
            [ withValidatorWhen (fun i -> i.id = null) validateNewIngredientProps
              withValidatorWhen (fun i -> i.id <> null) validateIngredientId ]
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
              eachItemWith validateIngredient ]

        validate
            (fun recipe -> recipe.equipments)
            [ isNotEmpty
              eachItemWith validateEquipment ]
    }
