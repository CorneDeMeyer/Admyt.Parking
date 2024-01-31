# Parking
.NET 8 C# ASP.NET Web REST API for collecting gate events in order to determine the parking fee required for a parking session..

## Tables Of Content
- [Badges](#Badges)
- [Technology](#Technology)
- [Features](#Features)
- [Usage](#Usage)
- [Issues](#Issues)
- [Contribution](#Contribution)
- [License](#License)

## Badges
![Build Status](https://github.com/CorneDeMeyer/Admyt.Parking/actions/workflows/dotnet.yml/badge.svg?branch=main)

## Technology
Making use of the following Libraries:
- SignalR
- Dapper
- Azure Queue

## Features

### Calculate Parking Fee
- Calculates the parking fee for an existing parking session based on a queried plate text.
- If the session is still active, the fee should be calculated based on the current time.
- If the session is completed, the fee should be calculated based on the time the session was completed.
- A session is considered completed if the last gate event was an exit event at depth 0.
- If the session is not found, the fee should be 0.

### Gate Event
- Receives a gate event from a gate device and stores it in the database.
- Matches/Creates a parking session based on the plate text.

## Usage
1. Clone the Repository
```bash
	https://github.com/CorneDeMeyer/Admyt.Parking.git
```
2. Run Build on the Main branch
3. Create seperate branch and have fun :)

## Issues
Report issues or ideas to make the application a better experience for users

## Contribution
1. Fork the repository.
2. Create a new branch: `git checkout -b feature-name`.
3. Make your changes.
4. Push your branch: `git push origin feature-name`.
5. Create a pull request.

## License
This project is licensed under the [MIT License](LICENSE).