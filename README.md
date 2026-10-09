# FocusFuel
Study productivity app built with Blazor Server (.NET 8), C# and Tailwind CSS. No database yet: data is saved in the browser's encrypted local storage.

## Run
Requires the .NET 8 SDK.

    dotnet run

The compiled stylesheet (`wwwroot/css/app.css`) is included. To edit styles, install Node and run:

    npm install
    npm run css:watch

## Pages
/login, /register, / (dashboard), /timer, /tasks, /goals, /profile, /settings

## Structure
- `Models/` entities: User, TimerSettings, StudySession, StudyTask, Achievement, UserAchievement, TimerMode
- `Services/AppState.cs` app logic and persistence
- `Components/` layouts, pages and shared components
- `docs/class-diagram.png` class diagram
