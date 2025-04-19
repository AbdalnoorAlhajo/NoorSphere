import React from "react";
import { formatDate } from "../../utils/global";
const Experience = ({ experience }) => {
  return (
    <div>
      {experience?.map((e, index) => (
        <div key={index} className="container">
          <p>
            &#128188; {e.current ? "Works" : "Worked"} as <b>{e.title}</b> at <b>{e.company}</b>
          </p>
          <small>
            from {formatDate(e.from)} to {e.current ? "Current" : formatDate(e.to)}
          </small>
        </div>
      ))}
    </div>
  );
};

export default Experience;
