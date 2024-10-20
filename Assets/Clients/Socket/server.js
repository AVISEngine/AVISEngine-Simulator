const express = require('express');
const http = require('http');
const socketIo = require('socket.io');

const app = express();
const server = http.createServer(app);
const io = socketIo(server);

io.on('connection', (socket) => {
    console.log('A user connected');

    socket.on('initialData', (data) => {
        console.log('Received initial data:', data);
        // Emit a response back to the client
        socket.emit('dataEvent', { message: 'Hello from server!' });
    });

    socket.on('disconnect', () => {
        console.log('User disconnected');
    });
});

const PORT = 4567; // Ensure this matches your Unity client
server.listen(PORT, () => {
    console.log(`Server is running on http://localhost:${PORT}`);
});