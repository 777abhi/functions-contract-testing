import express from 'express';
import cors from 'cors';
import { Request, Response } from 'express';

const app = express();
const PORT = process.env.PORT ?? 3000;

// Middleware
app.use(cors());
app.use(express.json());
app.use(express.urlencoded({ extended: true }));

// Health check endpoint
app.get('/health', (req: Request, res: Response) => {
  res.json({ 
    status: 'OK', 
    timestamp: new Date().toISOString(),
    uptime: process.uptime()
  });
});

// Mock API endpoints
app.get('/api/todos', (req: Request, res: Response) => {
  const todos = [
    {
      userId: 1,
      id: 1,
      title: "delectus aut autem",
      completed: false
    },
    {
      userId: 1,
      id: 2,
      title: "quis ut nam facilis et officia qui",
      completed: false
    },
    {
      userId: 1,
      id: 3,
      title: "fugiat veniam minus",
      completed: false
    }
  ];
  res.json(todos);
});
app.get('/api/users', (req: Request, res: Response) => {
  const users = [
    { id: 1, name: 'John Doe', email: 'john@example.com' },
    { id: 2, name: 'Jane Smith', email: 'jane@example.com' },
    { id: 3, name: 'Bob Johnson', email: 'bob@example.com' }
  ];
  res.json(users);
});

app.get('/api/users/:id', (req: Request, res: Response) => {
  const id = parseInt(req.params.id);
  const user = { id, name: `User ${id}`, email: `user${id}@example.com` };
  res.json(user);
});

app.post('/api/users', (req: Request, res: Response) => {
  const { name, email } = req.body;
  const newUser = {
    id: Math.floor(Math.random() * 1000),
    name: name || 'Anonymous',
    email: email || 'anonymous@example.com',
    createdAt: new Date().toISOString()
  };
  res.status(201).json(newUser);
});

app.put('/api/users/:id', (req: Request, res: Response) => {
  const id = parseInt(req.params.id);
  const { name, email } = req.body;
  const updatedUser = {
    id,
    name: name || `User ${id}`,
    email: email || `user${id}@example.com`,
    updatedAt: new Date().toISOString()
  };
  res.json(updatedUser);
});

app.delete('/api/users/:id', (req: Request, res: Response) => {
  const id = parseInt(req.params.id);
  res.json({ message: `User ${id} deleted successfully` });
});

// Mock data endpoints
app.get('/api/products', (req: Request, res: Response) => {
  const products = [
    { id: 1, name: 'Laptop', price: 999.99, category: 'Electronics' },
    { id: 2, name: 'Book', price: 19.99, category: 'Education' },
    { id: 3, name: 'Coffee Mug', price: 12.50, category: 'Home' }
  ];
  res.json(products);
});

app.get('/api/orders', (req: Request, res: Response) => {
  const orders = [
    { id: 1, userId: 1, total: 999.99, status: 'completed' },
    { id: 2, userId: 2, total: 32.49, status: 'pending' },
    { id: 3, userId: 1, total: 12.50, status: 'shipped' }
  ];
  res.json(orders);
});

// Webhook endpoint for testing
app.post('/api/webhook', (req: Request, res: Response) => {
  console.log('Webhook received:', req.body);
  res.json({ 
    message: 'Webhook received successfully',
    timestamp: new Date().toISOString(),
    data: req.body 
  });
});

// Error handling middleware
app.use((err: Error, req: Request, res: Response, next: any) => {
  console.error(err.stack);
  res.status(500).json({ error: 'Something went wrong!' });
});

// 404 handler
app.use('*', (req: Request, res: Response) => {
  res.status(404).json({ error: 'Endpoint not found' });
});

// Start server
app.listen(PORT, () => {
  console.log(`Mock server running on port ${PORT}`);
  console.log(`Health check: http://localhost:${PORT}/health`);
});

export default app;