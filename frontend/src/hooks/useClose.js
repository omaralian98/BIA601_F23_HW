import { useEffect, useRef, useState } from "react";

const useClose = () => {
  const [open, setOpen] = useState(false);
  const mouse = useRef();
  useEffect(() => {
    const handler = (e) => {
      if (mouse.current) {
        if (!mouse.current.contains(e.target)) {
          setOpen(false);
        }
      }
    };
    document.addEventListener("mousedown", handler);
    return () => {
      document.addEventListener("mousedown", handler);
    };
  }, []);
  return { open, setOpen, mouse };
};

export default useClose;
