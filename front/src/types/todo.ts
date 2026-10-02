export interface Todo {
  id: number;
  title: string;
  description: string | null;
  completed: boolean;
  updatedAt: string;
}

export interface LoginResponse {
  token: string;
  userId: number;
  email: string;
}

export interface RegisterResponse {
  message: string;
  userId: number;
  email: string;
}

export interface DeleteTaskResponse {
  message: string;
}
