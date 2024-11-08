FROM node:18-alpine AS build
WORKDIR /app

COPY ./DotaDashboard-FE/package.json ./DotaDashboard-FE/package-lock.json ./
RUN npm install --production

COPY ./DotaDashboard-FE/ ./
RUN npm run build

FROM node:18-alpine
WORKDIR /app

COPY --from=build /app ./

EXPOSE 3000

CMD ["npm", "run", "start"]
