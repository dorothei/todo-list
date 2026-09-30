import { useState } from "react";
import { createTask } from "../api/api";

function TaskForm({ onTaskCreated }) {
  const [title, setTitle] = useState("");
  const [description, setDescription] = useState("");
  const [error, setError] = useState("");

  async function handleSubmit(event) {
    event.preventDefault();

    setError("");

    if (!title.trim()) {
      setError("Введите название задачи");
      return;
    }

    try {
      const task = await createTask(title.trim(), description.trim());

      onTaskCreated(task);

      setTitle("");
      setDescription("");
    } catch (error) {
      console.error(error);
      setError("Не удалось создать задачу");
    }
  }

  return (
    <form className="task-form" onSubmit={handleSubmit}>
      <input
        type="text"
        placeholder="Название задачи"
        value={title}
        onChange={(event) => {
          setTitle(event.target.value);

          if (error) {
            setError("");
          }
        }}
      />

      <input
        type="text"
        placeholder="Описание"
        value={description}
        onChange={(event) => setDescription(event.target.value)}
      />

      {error && <div className="form-error">{error}</div>}

      <button type="submit">Добавить</button>
    </form>
  );
}

export default TaskForm;
