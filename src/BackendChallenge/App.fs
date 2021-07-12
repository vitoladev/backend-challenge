module BackendChallenge.App

open Giraffe
open BackendChallenge.HttpHandlers
open Microsoft.AspNetCore.Http

let webApp : HttpFunc -> HttpContext -> HttpFuncResult =
    choose [ subRoute
                 "/api"
                 (choose [ GET
                           >=> choose [ route "/hello" >=> handleGetHello ] ])
             setStatusCode 404 >=> text "Not Found" ]
