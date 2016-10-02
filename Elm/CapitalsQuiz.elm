import Html exposing (..)
import Html.App as App
import Html.Attributes exposing (..)
import Html.Events exposing (..)
import Http
import Task
import String exposing(..)
import Json.Decode exposing (..)

main : Program Never
main =
  App.program
    { init = (Model (Quiz "" "") "type your answer" [] [], getNewQuiz)
    , view = view
    , update = update
    , subscriptions = \_ -> Sub.none
    }

type Msg = Answer String 
         | Check
         | FetchSucceed Quiz
         | FetchFail Http.Error

type alias Quiz = 
  { country : String
  , capital : String
  } 

type alias Model =
  { quiz   : Quiz
  , answer : String
  , wins   : List String
  , losses : List String    
  }

view : Model -> Html Msg
view model =
  div []
    [ span [] [ text ("What is the capital of " ++ model.quiz.country ++ "? ") ]
    , model.answer |> \s -> input [ type' "text", placeholder "type your answer", Html.Attributes.value s, onInput Answer ] []
    , br [] []
    , button [ onClick Check ] [ text "Check" ]    
    , toHtmlList "green" model.wins
    , toHtmlList "red"  model.losses
    , div [style [ ("color", "white")]] [ text model.quiz.capital ] -- cheat
    ]

update : Msg -> Model -> (Model, Cmd Msg)
update msg model =
  case msg of
    Answer a ->
      { model | answer = a } ! [] 
    Check ->
      let message = "The capital of " ++ model.quiz.country ++ " is " ++ model.quiz.capital
      in
        if toUpper model.answer == toUpper model.quiz.capital then
          { model | wins = message :: model.wins } ! [getNewQuiz]
        else
          { model | losses = message :: model.losses } ! [getNewQuiz]  
    FetchSucceed q ->
      { model | quiz = q, answer = "" } ! []
    FetchFail e ->
        model ! []
        
getNewQuiz: Cmd Msg
getNewQuiz = Task.perform FetchFail FetchSucceed (Http.get decode "https://funcxz.azurewebsites.net/api/capitalsquiz")

decode : Decoder Quiz
decode = object2 Quiz ("Country" := string) ("Capital" := string)

toHtmlList : String -> List String -> Html Msg
toHtmlList color strings = ul [] (strings |> List.map (\s -> li [style [ ("color", color)]] [ text s ]))