import type { MouseEvent } from "react";
import type { Todo } from "../types/todo";

interface TaskCardProps {
  task: Todo;
  selected: boolean;
  onSelect: (task: Todo) => void;
  onToggle: (task: Todo) => void;
}

function TaskCard({
  task,
  selected,
  onSelect,
  onToggle,
}: TaskCardProps) {
  function handleToggle(event: MouseEvent<HTMLButtonElement>) {
    event.stopPropagation();

    onToggle(task);
  }

  return (
    <div
      className={`task-row ${selected ? "selected" : ""}`}
      onClick={() => onSelect(task)}
    >
      <button
        className={`task-checkbox ${task.completed ? "checked" : ""}`}
        onClick={handleToggle}
        aria-label="Изменить статус задачи"
      >
        {task.completed ? "✓" : ""}
      </button>

      <div className="task-row-content">
        <div className={`task-row-title ${task.completed ? "completed" : ""}`}>
          {task.title}
        </div>

        {task.description && (
          <div className="task-row-description">{task.description}</div>
        )}
      </div>
    </div>
  );
}

export default TaskCard;
