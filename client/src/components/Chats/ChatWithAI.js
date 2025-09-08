import { useEffect, useState } from "react";
import TypingText from "./TypingText";
import { ChatWithAIapi } from "../../utils/APIs/messagesService";
import { useToken } from "../TokenContext";

const ChatWithAI = () => {
  const [messages, setMessages] = useState([]);
  const [input, setInput] = useState("");
  const [gettingResponse, setGettingResponse] = useState(false);
  const { token } = useToken();
  const [AiResponse, setAiResponse] = useState("");

  useEffect(() => {
    const fetchAIResponse = async () => {
      if (messages.length === 0 || messages[messages.length - 1].role !== "user") return;

      setGettingResponse(true);
      try {
        const response = await ChatWithAIapi(token, messages);
        setAiResponse(response || "No response from AI.");
        setMessages((prev) => [...prev, { role: "model", parts: [{ text: response || "No response from AI." }] }]);
      } catch (error) {
        console.error("Error generating post with AI:", error);
      } finally {
        setGettingResponse(false);
      }
    };

    fetchAIResponse();
  }, [messages, token]);

  const handleSendMessage = (message) => {
    setMessages((prev) => [...prev, { role: "user", parts: [{ text: message }] }]);
    setInput("");
  };

  return (
    <div className="my-6 h-full p-10 border-2 border-solid border-[--primary-color] rounded-lg">
      <div className="flex">
        <svg
          xmlns="http://www.w3.org/2000/svg"
          fill="none"
          viewBox="0 0 24 24"
          strokeWidth={1.5}
          stroke="currentColor"
          className="size-8 text-[--primary-color] mr-3"
        >
          <path
            strokeLinecap="round"
            strokeLinejoin="round"
            d="M9.813 15.904 9 18.75l-.813-2.846a4.5 4.5 0 0 0-3.09-3.09L2.25 12l2.846-.813a4.5 4.5 0 0 0 3.09-3.09L9 5.25l.813 2.846a4.5 4.5 0 0 0 3.09 3.09L15.75 12l-2.846.813a4.5 4.5 0 0 0-3.09 3.09ZM18.259 8.715 18 9.75l-.259-1.035a3.375 3.375 0 0 0-2.455-2.456L14.25 6l1.036-.259a3.375 3.375 0 0 0 2.455-2.456L18 2.25l.259 1.035a3.375 3.375 0 0 0 2.456 2.456L21.75 6l-1.035.259a3.375 3.375 0 0 0-2.456 2.456ZM16.894 20.567 16.5 21.75l-.394-1.183a2.25 2.25 0 0 0-1.423-1.423L13.5 18.75l1.183-.394a2.25 2.25 0 0 0 1.423-1.423l.394-1.183.394 1.183a2.25 2.25 0 0 0 1.423 1.423l1.183.394-1.183.394a2.25 2.25 0 0 0-1.423 1.423Z"
          />
        </svg>
        <h1 className="text-2xl text-[--primary-color] font-bold">Chat with AI</h1>
      </div>
      <hr className="my-4 border-[--primary-color]" />
      <div>
        {messages.map((msg, index) => (
          <div>
            {msg.role === "user" ? (
              <div key={index} className="flex break-words">
                <p className="bg-slate-900 whitespace-pre-line rounded-xl  shadow-lg shadow-blue-500/100 text-[--primary-color] p-5 px-8 w-[400px]">
                  {msg.parts[0].text}
                </p>
              </div>
            ) : (
              <TypingText text={AiResponse} />
            )}
          </div>
        ))}

        {gettingResponse && <TypingText text="AI is typing..." />}
        <hr className="border-[--primary-color] mb-10" />
      </div>
      <div className="mt-10 flex gap-4">
        <input
          type="text"
          placeholder="Type your message..."
          value={input}
          onChange={(e) => setInput(e.target.value)}
          className="w-[80%] p-3 border-2 border-solid border-[--primary-color] rounded-lg bg-[--primary-color] text-slate-900 placeholder:text-[--secondary-color]"
        />
        <button disabled={gettingResponse} className="btn btn-primary" onClick={() => handleSendMessage(input)}>
          {gettingResponse ? "Waiting..." : "Send"}
        </button>
      </div>
    </div>
  );
};

export default ChatWithAI;
