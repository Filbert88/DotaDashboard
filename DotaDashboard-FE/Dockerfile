FROM node:18 AS build
WORKDIR /app

# Copy only the package files and install dependencies (helps with caching)
COPY ./DotaDashboard-FE/package.json ./DotaDashboard-FE/package-lock.json ./
RUN npm install

# Copy the rest of the frontend files and build
COPY ./DotaDashboard-FE/ ./
RUN npm run build

# Use a lightweight server to serve the built frontend
FROM node:18
WORKDIR /app
COPY --from=build /app .

# Expose the frontend port
EXPOSE 3000

# Run Next.js frontend
CMD ["npm", "run", "start"]
