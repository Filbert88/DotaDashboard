# Dota Dashboard

Dota Dashboard API is an application designed to provide Dota 2 hero suggestions, statistics, and recommendations based on OpenDota data. This API allows users to fetch hero stats by tier, professional stats, and personalized hero recommendations for individual players.

![GitHub last commit](https://img.shields.io/github/last-commit/Filbert88/DotaDashboard)

## Class Diagram
Link : https://drive.google.com/file/d/1aoxxOLlqAukfEmBRNrnbbfRAoP_KNvhP/view?usp=sharing 

## Features
- **Hero Tier Suggestions**: View top heroes by skill tiers (e.g., Herald, Guardian, Crusader), computed using win rate, pick rate, and ban rate.
- **Professional Hero Stats**: Access data on the most picked and banned heroes in professional Dota 2 matches.
- **Player-Specific Recommendations**: Get hero recommendations tailored to an individual player's play history, favorite heroes, and in-game performance.

## Tech Stack
- **Frontend**: Built with Next.js, Tailwind CSS, and shadcn for UI styling.
- **Backend**: Developed using .NET for robust API handling.

## Technologies Used
- **ASP.NET Core**: Backend framework for creating RESTful APIs
- **OpenDota API**: Source for hero and player statistics
- **C#**: Core programming language
- **Docker**: Containerization for easy deployment 

## Getting Started

### Prerequisites
- Docker and Docker Compose (if you want to run with Docker)
- .NET SDK (if you want to run the backend locally)
- Node.js and npm (for running the frontend locally)

### Clone the Repository
Clone this repository to your local machine:
```bash
git clone https://github.com/Filbert88/DotaDashboard.git
cd DotaDashboard
```

## Running the Application

### Option 1: Run with Docker
You can easily run both the frontend and backend services using Docker Compose.
```bash
docker-compose up --build
```

### Option 2: Run Locally without Docker

1. **Backend**:
   - Navigate to the backend folder:
     ```bash
     cd DotaDashboard-BE/DotaDashboardAPI
     ```
   - Install dependencies (if necessary):
     ```bash
     dotnet restore
     ```
   - Run the backend:
     ```bash
     dotnet run
     ```

2. **Frontend**:
   - Open a new terminal and navigate to the frontend folder:
     ```bash
     cd DotaDashboard-FE
     ```
   - Install dependencies:
     ```bash
     npm install
     ```
   - Start the frontend development server:
     ```bash
     npm run dev
     ```

Then, you access the web in `http://localhost:3000`.

## Links
- Repository : https://github.com/Filbert88/DotaDashboard.git
- Issue tracker :
  - If you encounter any issues with the program, come across any disruptive bugs, or have any suggestions for improvement, please don't hesitate to reach out by sending an email to filbertfilbert21@gmail.com. Your feedback is greatly appreciated.