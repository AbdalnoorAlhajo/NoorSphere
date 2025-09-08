import { useState, useEffect } from "react";

const TypingText = ({ text = "", speed = 30 }) => {
  const [displayedText, setDisplayedText] = useState("");
  const [index, setIndex] = useState(0);
  useEffect(() => {
    if (!text) return;

    const interval = setInterval(() => {
      setDisplayedText((prev) => prev + text.charAt(index));
      setIndex((prev) => prev + 1);
      if (index >= text.length) clearInterval(interval);
    }, speed);

    return () => clearInterval(interval);
  }, [text, speed, index]);

  return (
    <div className="flex break-words justify-end my-10">
      <p className="whitespace-pre-line w-[400px] bg-[--secondary-color] text-[--primary-color] p-[12px] shadow-lg shadow-blue-500/100 rounded-xl">
        {displayedText}
      </p>
    </div>
  );
};

export default TypingText;
