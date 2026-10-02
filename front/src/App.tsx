import "./App.css";
import { BrowserRouter, Navigate, Route, Routes } from "react-router-dom";

import Login from "./pages/Login";
import Register from "./pages/Register";
import Tasks from "./pages/Tasks";
import type { FC, PropsWithChildren } from "react";

const ProtectedRoute: FC<PropsWithChildren> = ({ children }) => {
  return localStorage.getItem("token") ? children : <Navigate to="/login" replace />;
};

const PublicOnlyRoute: FC<PropsWithChildren> = ({ children }) => {
  return localStorage.getItem("token") ? <Navigate to="/" replace /> : children;
};

function App() {
  return (
    <BrowserRouter>
      <Routes>
        <Route
          path="/login"
          element={
            <PublicOnlyRoute>
              <Login />
            </PublicOnlyRoute>
          }
        />
        <Route
          path="/register"
          element={
            <PublicOnlyRoute>
              <Register />
            </PublicOnlyRoute>
          }
        />
        <Route
          path="/"
          element={
            <ProtectedRoute>
              <Tasks />
            </ProtectedRoute>
          }
        />
        <Route path="*" element={<Navigate to="/" replace />} />
      </Routes>
    </BrowserRouter>
  );
}

export default App;
