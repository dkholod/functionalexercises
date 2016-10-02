module Main exposing (..)

import Html.App as App
import Html exposing (..)
import Html.Attributes exposing (href, class, style)
import Material
import Material.Scheme
import Material.Button as Button
import Material.Options exposing (css)
import Material.Textfield as Textfield
import Material.List as Lists
import Http
import Task
import String exposing (..)
import Json.Decode exposing (..)


type Msg
    = Answer String
    | Check
    | FetchSucceed Quiz
    | FetchFail Http.Error
    | Mdl (Material.Msg Msg)


type alias Quiz =
    { country : String
    , capital : String
    }


type alias Model =
    { quiz : Quiz
    , answer : String
    , statistics : List Status
    , status : Status
    , mdl : Material.Model
    }


type Status
    = None
    | Right String
    | Wrong String


type alias Mdl =
    Material.Model


view : Model -> Html Msg
view model =
    div []
        [ h1 [ style [ ( "color", "CadetBlue " ) ] ] [ text "Capitals & Countries <| quiz" ]
        , div [ style [ ( "padding", "2rem" ) ] ]
            [ text ("What is the capital of " ++ model.quiz.country ++ ": ")
            , Textfield.render Mdl
                [ 2 ]
                model.mdl
                [ Textfield.label "the answer"
                , Textfield.onInput Answer
                , Textfield.value model.answer
                , Textfield.floatingLabel
                , Textfield.text'
                ]
            , Button.render Mdl
                [ 1 ]
                model.mdl
                [ Button.raised
                , Button.colored
                , Button.onClick Check
                , css "margin" "0 24px"
                ]
                [ text "Check" ]
            ]
        --, score model.statistics
        , status model.status
        , list model.statistics
        , div [ style [ ( "color", "white" ) ] ] [ text model.quiz.capital ] -- cheat
        ]
        |> Material.Scheme.top

--score stats = 
--  let wins = stats |> List.filter (\it -> ) 

status : Status -> Html a
status st =
    let
        ( color, txt ) =
            case st of
                Right s ->
                    ( "green", "Right. " ++ s )

                Wrong s ->
                    ( "red", "Wrong! " ++ s )

                None ->
                    ( "", "" )
    in
        div [ style [ ( "color", color ) ] ] [ text txt ]


list : List Status -> Html a
list items =
    let
        f =
            \it ->
                case it of
                    Right txt ->
                        ( Lists.icon "done" [], txt )

                    Wrong txt ->
                        ( Lists.icon "error outline" [], txt )

                    None ->
                        ( Lists.icon "" [], "" )
    in
        Lists.ul []
            (items
                |> List.map f
                |> List.map (\( icon, txt ) -> Lists.li [] [ Lists.content [] [ icon, text txt ] ])
            )


update : Msg -> Model -> ( Model, Cmd Msg )
update msg model =
    case msg of
        Answer a ->
            { model | answer = a, status = None } ! []

        Check ->
            let
                message =
                    "The capital of " ++ model.quiz.country ++ " is " ++ model.quiz.capital
            in
                let
                    checkStatus =
                        if toUpper model.answer == toUpper model.quiz.capital then
                            Right message
                        else
                            Wrong message
                in
                    { model | statistics = checkStatus :: model.statistics, status = checkStatus } ! [ getNewQuiz ]

        FetchSucceed q ->
            if q.capital == "" then
                model ! [ getNewQuiz ]
            else
                { model | quiz = q, answer = "" } ! []

        FetchFail e ->
            model ! []

        Mdl msg' ->
            Material.update msg' model


getNewQuiz : Cmd Msg
getNewQuiz =
    Task.perform FetchFail FetchSucceed (Http.get decode "https://funcxz.azurewebsites.net/api/capitalsquiz")


decode : Decoder Quiz
decode =
    object2 Quiz ("Country" := string) ("Capital" := string)


model : Model
model =
    { quiz = (Quiz "" "")
    , answer = ""
    , statistics = []
    , mdl = Material.model
    , status = None
    }


main : Program Never
main =
    App.program
        { init = ( model, getNewQuiz )
        , view = view
        , subscriptions = always Sub.none
        , update = update
        }
