import React, { useEffect, useRef, useState } from "react";
import * as d3 from "d3";
import useFormData from "../../store";

// const preprocessLinks = (links) => {
//   const uniqueLinks = {};

//   links.forEach((link) => {
//     const key = `${link.source}-${link.target}`;
//     const reverseKey = `${link.target}-${link.source}`;

//     if (uniqueLinks[key]) {
//       uniqueLinks[key] = Math.min(uniqueLinks[key], link.distance);
//     } else if (uniqueLinks[reverseKey]) {
//       uniqueLinks[reverseKey] = Math.min(
//         uniqueLinks[reverseKey],
//         link.distance
//       );
//     } else {
//       uniqueLinks[key] = link.distance;
//     }
//   });

//   return Object.entries(uniqueLinks).map(([key, distance]) => {
//     const [source, target] = key.split("-");
//     return { source: source, target: target, distance: distance };
//   });
// };

const Graph = () => {
  const { response } = useFormData();

  const { formDataStorage } = useFormData();
  const [nodes, setNodes] = useState([]);
  const [links, setLinks] = useState([]);
  const svgRef = useRef(null);
  let locations = [];
  let temp2 = [];

  const nodesData = formDataStorage?.locations?.locations_name?.map((e) => ({
    id: e,
    label: e,
  }));

  for (let i = 0; i < response?.bestRoute?.length; i++) {
    let start;
    let end;
    start =
      formDataStorage?.locations?.locations_name[
        parseInt(response.bestRoute[i])
      ];
    end =
      formDataStorage?.locations?.locations_name[
        parseInt(response?.bestRoute[i + 1])
      ];
    locations.push({ start, end });
  }
  locations.map((location, i) => {
    let x = {};
    formDataStorage?.locations?.distances?.map((e) =>
      e.map((d) => {
        if (
          d.start === location.start &&
          d.end === (location.end ? location.end : location.start)
        ) {
          x = {
            source: d.start,
            target: d.end,
            distance: d.distance,
          };
          temp2.push(x);
        }
      })
    );
  });

  // const initialLinks = formDataStorage?.locations?.distances?.flatMap(
  //   (object) =>
  //     object
  //       .filter((e) => e.start !== e.end)
  //       .map((element) => ({
  //         source: element.start,
  //         target: element.end,
  //         distance: element.distance,
  //       }))
  // );
  // const linksData = preprocessLinks(initialLinks);

  useEffect(() => {
    const nodes = nodesData.map((node, index) => ({ id: index, ...node }));
    const links = temp2.map((link) => ({
      source: link.source,
      target: link.target,
      distance: link.distance,
    }));

    const simulation = d3
      .forceSimulation(nodes)
      .force(
        "link",
        d3
          .forceLink(links)
          .distance((d) => d.distance)
          .id((d) => d.id)
      )
      .force("charge", d3.forceManyBody().strength(-300))
      .force("center", d3.forceCenter(300, 300)) // Center the simulation
      .on("tick", () => {
        setNodes([...nodes]);
        setLinks([...links]);
      });

    return () => {
      simulation.stop();
    };
  }, [locations, response]);

  return (
    <div className="flex justify-center items-center w-full h-full">
      <svg ref={svgRef} width={600} height={600}>
        {links.map((link, index) => (
          <line
            key={index}
            x1={link.source.x}
            y1={link.source.y}
            x2={link.target.x}
            y2={link.target.y}
            stroke="black"
          />
        ))}
        {nodes.length > 0 &&
          nodes.map((node, index) => (
            <g key={index}>
              <circle cx={node.x} cy={node.y} r={7} fill="red" />
              <text
                x={node.x}
                y={node.y}
                dy="-1em"
                textAnchor="bottom"
                fontSize="8px"
                fontWeight="bold"
                fill="black">
                {node.label}
              </text>
            </g>
          ))}
      </svg>
    </div>
  );
};

export default Graph;
